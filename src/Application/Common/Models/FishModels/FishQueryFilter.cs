﻿namespace Application.Common.Models.FishModels;

public class FishQueryFilter
{
    public string? Search { get; set; }

    public string? Breed { get; set; }
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