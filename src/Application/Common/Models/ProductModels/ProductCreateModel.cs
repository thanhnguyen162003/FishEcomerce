using Application.Common.Models.TankModels;

namespace Application.Common.Models.ProductModels;

public class ProductCreateModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? DescriptionDetail { get; set; }

    public string? Type { get; set; }

    public Guid? SupplierId { get; set; }

    public int? StockQuantity { get; set; }
    
    public decimal? Price { get; set; }
    
    public decimal? OriginalPrice { get; set; }
    
    public TankCreateModel? TankModel { get; set; }
}