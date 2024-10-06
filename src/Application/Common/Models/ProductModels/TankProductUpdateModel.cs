using Application.Common.Models.TankModels;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Models.ProductModels;

public class TankProductUpdateModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? DescriptionDetail { get; set; }
    
    public int? StockQuantity { get; set; }
    
    public decimal? Price { get; set; }
    
    public decimal? OriginalPrice { get; set; }
    
    public IEnumerable<Guid> DeleteImages { get; set; } = new List<Guid>();
    
    public IEnumerable<IFormFile> UpdateImages { get; set; }
    
    public TankUpdateModel? TankModel { get; set; }
}