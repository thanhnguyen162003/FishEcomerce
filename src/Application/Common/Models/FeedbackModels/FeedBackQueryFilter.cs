namespace Application.Common.Models.FeedbackModels;

public class FeedBackQueryFilter
{
    public string? Sort { get; set; } = "date";

    public string? Direction { get; set; } = "asc";
    
    public int? Rate { get; set; }
    
    public int PageSize { get; set; }

    public int PageNumber { get; set; }
    
    public void ApplyDefaults()
    {
        if (string.IsNullOrWhiteSpace(Sort))
        {
            Sort = "date";
        }
        
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = "asc";
        }
    }
}