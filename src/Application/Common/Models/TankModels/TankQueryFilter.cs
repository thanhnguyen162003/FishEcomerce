namespace Application.Common.Models.TankModels;

public class TankQueryFilter
{
    public string? Search {get; set;}
    
    public string? Category {get; set;}

    public string? Sort { get; set; } = "date";
    
    public string? Direction { get; set; } = "asc";
    
    public int PageSize { get; set; }

    public int PageNumber { get; set; }
}