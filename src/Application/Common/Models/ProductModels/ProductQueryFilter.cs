namespace Application.Common.Models.ProductModels;

public class ProductQueryFilter
{
    public string? Search {get; set;}
    
    public string? Category {get; set;}
    
    public decimal? PriceFrom { get; set; }
    
    public decimal? PriceTo { get; set; }
    
    public string? Sort { get; set; }
    
    public string? Direction { get; set; }

    public int? PageSize { get; set; }

    public int? PageNumber { get; set; }
    
    public void ApplyDefaults()
    {
        if (string.IsNullOrWhiteSpace(Sort))
        {
            Sort = "price";
        }
        
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = "asc";
        }
    }
}