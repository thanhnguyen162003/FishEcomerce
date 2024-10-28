using Application.Common.Models.OrderDetailModels;
using Domain.Entites;
using Domain.Enums;

namespace Application.Common.Models.OrderModels;

public class OrderCreateModel
{
    public decimal? TotalPrice { get; set; }
    
    public PaymentMethod? PaymentMethod { get; set; }
    
    public string? FullName { get; set; }

    public string? ShipAddress { get; set; }
    
    public List<OrderDetailCreateModel> OrderDetails  { get; set; }
}