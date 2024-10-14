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
    bool SignatureValidate(WebhookType webhookType);
}

public class PayOSService : IPayOSService
{
    private readonly Net.payOS.PayOS _payOs;
    private readonly string _checksumKey;

    public PayOSService(IOptions<PayOSSettings> config)
    {
        _payOs = new Net.payOS.PayOS(
            config.Value.ClientID, config.Value.APIKey, config.Value.ChecksumKey);
        _checksumKey = config.Value.ChecksumKey;
    }

    public async Task<string> CreatePayment(PaymentRequestModel model)
    {
        var orderCode = int.Parse(DateTimeOffset.Now.ToString("fffff"));

        var data =
            $"amount={model.TotalPrice}&cancelUrl={"https://localhost:7158/api/v1/fail"}&description={model.Description}&orderCode={orderCode}&returnUrl={"https://localhost:7158/api/v1/success"}";

        var signature = CreateSignature(data, _checksumKey);

        var expired = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds();

        PaymentData paymentData = new PaymentData(
            orderCode,
            (int)model.TotalPrice,
            model.Description,
            [],
            "https://localhost:7158/api/v1/fail",
            "https://localhost:7158/api/v1/success",
            signature,
            model.FullName,
            "",
            "",
            model.Address,
            expired
        );

        var result = await _payOs.createPaymentLink(paymentData);
        return result.checkoutUrl;
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

    private string CreateSignature(string data, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        using var hmacsha256 = new HMACSHA256(keyBytes);
        byte[] hashBytes = hmacsha256.ComputeHash(dataBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}