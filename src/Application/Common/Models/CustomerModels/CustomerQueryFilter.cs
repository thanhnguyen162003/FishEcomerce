namespace Application.Common.Models.CustomerModels;

public class CustomerQueryFilter
{
    public string? Search { get; set; }
    
    public string? Sort { get; set; }
    
    public string? Direction { get; set; } 
    
    public int? PageSize { get; set; }
    
    public int? PageNumber { get; set; }
    
    public void ApplyDefaults()
    {
        if (string.IsNullOrWhiteSpace(Sort))
        {
            Sort = "name";
        }
        
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = "asc";
        }
    }
}