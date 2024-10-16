using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models.PaymentModels;

public class PaymentRequestModel
{
    public long OrderCode { get; set; }
    public decimal TotalPrice { get; set; }
    public string Description { get; set; }
    public string FullName { get; set; }
    
    // public string Email { get; set; }
    //
    // public string Phone { get; set; }
    public string Address { get; set; }
    
}