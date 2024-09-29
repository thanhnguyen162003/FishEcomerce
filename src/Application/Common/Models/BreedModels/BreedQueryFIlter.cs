using Application.Common.Models.TankModels;

namespace Application.Common.Models.BreedModels;

public class BreedQueryFilter
{
    public string? Search { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

}