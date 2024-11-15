using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Models.ProductModels;

public class ProductUpdateModel
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? DescriptionDetail { get; set; }

    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? OriginalPrice { get; set; }
    
    public CategoryType? Type { get; set; }
    
    public IEnumerable<Guid> DeleteCategories { get; set; } = new List<Guid>();
    
    public IEnumerable<Guid> UpdateCategories { get; set; } = new List<Guid>();
    
    public IEnumerable<Guid>? DeleteImages { get; set; }
    
    public IEnumerable<IFormFile>? UpdateImages { get; set; }
}