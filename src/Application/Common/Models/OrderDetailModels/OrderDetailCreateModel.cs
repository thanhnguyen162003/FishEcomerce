namespace Application.Common.Models.OrderDetailModels;

public class OrderDetailCreateModel
{
    
    public Guid? ProductId { get; set; }
    
    public int? Quantity { get; set; }
    
    public decimal? UnitPrice { get; set; }
    
    public decimal? TotalPrice { get; set; }
    
    public decimal? Discount { get; set; }
}