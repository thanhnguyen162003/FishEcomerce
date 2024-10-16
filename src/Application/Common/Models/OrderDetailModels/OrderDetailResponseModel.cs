namespace Application.Common.Models.OrderDetailModels;

public class OrderDetailResponseModel
{
    public string? ProductName { get; set; }

    public int? Quantity { get; set; }
    
    public decimal? UnitPrice { get; set; }
    
    public decimal? TotalPrice { get; set; }
    
    public decimal? Discount { get; set; }
}