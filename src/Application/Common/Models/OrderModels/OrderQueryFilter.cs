namespace Application.Common.Models.OrderModels;

public class OrderQueryFilter
{
    public string? Direction { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }
    
    public void ApplyDefaults()
    {
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = "asc";
        }
    }
}