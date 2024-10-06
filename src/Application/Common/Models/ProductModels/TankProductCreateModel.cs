using Application.Common.Models.TankModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Common.Models.ProductModels;

public class TankProductCreateModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? DescriptionDetail { get; set; }
    
    public int? StockQuantity { get; set; }
    
    public decimal? Price { get; set; }
    
    public decimal? OriginalPrice { get; set; }
    
    [JsonIgnore]
    public IEnumerable<IFormFile>? ImageFiles { get; set; }
    
    public TankCreateModel? TankModel { get; set; }
    
    public IEnumerable<Guid>? CategoriesIds { get; set; }
}