using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Models.ProductModels;

public class FishProductUpdateModel
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? DescriptionDetail { get; set; }

    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? OriginalPrice { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    public IEnumerable<Guid>? DeleteImages { get; set; }
    public IEnumerable<IFormFile>? UpdateImages { get; set; }

    public FishUpdateRequestModel? FishModel { get; set; }
    
}