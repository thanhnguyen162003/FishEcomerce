using System.Security.Cryptography;
using System.Text;
using Application.Common.Models.PaymentModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;

namespace Application.Common.ThirdPartyManager.PayOS;

public interface IPayOSService
{
    Task<string> CreatePayment(PaymentRequestModel model);
    Task<PaymentLinkInformation> CancelPayment(long orderCode);
    bool SignatureValidate(WebhookType webhookType);
}

public class PayOSService : IPayOSService
{
    private readonly Net.payOS.PayOS _payOs;
    private readonly string _checksumKey;
    const string returlUrl = "https://fish-ecomerce-fe.vercel.app/payos";
    public PayOSService(IOptions<PayOSSettings> config)
    {
        _payOs = new Net.payOS.PayOS(
            config.Value.ClientID, config.Value.APIKey, config.Value.ChecksumKey);
        _checksumKey = config.Value.ChecksumKey;
    }

    public async Task<string> CreatePayment(PaymentRequestModel model)
    {

        // var data =
        //     $"amount={model.TotalPrice}&cancelUrl={returlUrl}&description={model.Description}&orderCode={model.OrderCode}&returnUrl={returlUrl}";
        //
        // var signature = CreateSignature(data, _checksumKey);

        var expired = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

        var paymentData = new PaymentData(
            model.OrderCode,
            (int)model.TotalPrice,
            model.Description,
            [],
            returlUrl,
            returlUrl,
            "",
            model.FullName,
            "",
            "",
            model.Address,
            expired
        );

        var result = await _payOs.createPaymentLink(paymentData);
        return result.checkoutUrl;
    }

    public async Task<PaymentLinkInformation> CancelPayment(long orderCode)
    {
        return await _payOs.cancelPaymentLink(orderCode);
    }

    public bool SignatureValidate(WebhookType webhookType)
    {
        try
        {
            _payOs.verifyPaymentWebhookData(webhookType);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    // private string CreateSignature(string data, string key)
    // {
    //     byte[] keyBytes = Encoding.UTF8.GetBytes(key);
    //     byte[] dataBytes = Encoding.UTF8.GetBytes(data);
    //
    //     using var hmacsha256 = new HMACSHA256(keyBytes);
    //     byte[] hashBytes = hmacsha256.ComputeHash(dataBytes);
    //     return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    // }
}