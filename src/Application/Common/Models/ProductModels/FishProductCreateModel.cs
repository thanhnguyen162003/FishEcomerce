using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Application.Common.Models.ProductModels;

public class FishProductCreateModel
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? DescriptionDetail { get; set; }

    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }

    public decimal? OriginalPrice { get; set; }
    [JsonIgnore]
    public IEnumerable<IFormFile>? ImageFiles { get; set; }

    public FishCreateRequestModel? FishModel { get; set; }
    public IEnumerable<FishAwardCreateRequestModel>? FishAward { get; set; }
}