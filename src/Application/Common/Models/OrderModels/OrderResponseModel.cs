using Application.Common.Models.OrderDetailModels;

namespace Application.Common.Models.OrderModels;

public class OrderResponseModel
{
    public DateOnly? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? TotalPrice { get; set; }
    
    public string? Status { get; set; }
    
    public long? OrderCode { get; set; }
    
    public bool? IsPaid { get; set; }
    
    public string? PaymentMethod { get; set; }

    public string? ShipAddress { get; set; }
    
    public IEnumerable<OrderDetailResponseModel> OrderDetails { get; set; }

}