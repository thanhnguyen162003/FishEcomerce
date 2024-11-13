using Domain.Enums;

namespace Application.Common.Models.CategoryModels;

public class CategoryQueryFilter
{
    public CategoryType? CategoryType { get; set; }
    
    public int? PageSize { get; set; }

    public int? PageNumber { get; set; }
}