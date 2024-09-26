namespace FishEcomerce.Application.Common.Models.ProductModels;

public class ProductRequestModel
{
    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    public int StockQuantity { get; set; }

    public decimal Price { get; set; }
}
