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
    Task<bool> SignatureValidate(int orderCode, string transactionSignature);
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
            model.Email,
            model.Phone,
            model.Address,
            expired
        );

        var result = await _payOs.createPaymentLink(paymentData);
        return result.checkoutUrl;
    }

    public async Task<bool> SignatureValidate(int orderCode, string transactionSignature)
    {
        try
        {
            var transaction = await _payOs.getPaymentLinkInformation(orderCode);

            var info = transaction.ToString();
            
            JObject jsonObject = JObject.Parse(info.Replace('\'', '\"'));
            var sortedKeys = SortedKeys(jsonObject);

            StringBuilder transactionStr = new StringBuilder();
            foreach (var key in sortedKeys)
            {
                string value = jsonObject[key].ToString();
                transactionStr.Append(key);
                transactionStr.Append('=');
                transactionStr.Append(value);
                if (key != sortedKeys[^1])
                {
                    transactionStr.Append('&');
                }
            }

            string signature = CreateSignature(transactionStr.ToString(), _checksumKey);
            return signature.Equals(transactionSignature, StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    
    private List<string> SortedKeys(JObject jsonObject)
    {
        List<string> keys = new List<string>();
        foreach (var key in jsonObject)
        {
            keys.Add(key.Key);
        }

        keys.Sort(); // Sắp xếp theo alphabet
        return keys;
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