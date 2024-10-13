using Application.Common.Models.OrderDetailModels;
using Domain.Entites;

namespace Application.Common.Models.OrderModels;

public class OrderCreateModel
{

    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? TotalPrice { get; set; }
    
    public string? Status { get; set; }
    
    public string? PaymentMethod { get; set; }

    public string? ShipAddress { get; set; }
    
    public List<OrderDetailCreateModel> OrderDetails  { get; set; }
}