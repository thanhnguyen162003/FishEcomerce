namespace Application.Common.Models.PaymentModels;

public class WebhookModel
{
    public bool Success { get; set; }
    public int OrderCode { get; set; }
    public string Signature { get; set; }
}