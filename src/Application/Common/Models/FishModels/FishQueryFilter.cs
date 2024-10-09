namespace Application.Common.Models.FishModels;

public class FishQueryFilter
{
    public string? Search { get; set; }

    public string? Breed { get; set; }

    public string? Sort { get; set; }

    public string? Direction { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }
}