namespace FishEcomerce.Application.Common.Models.ProductModels;

public class ProductUpdateRequestModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }
}
