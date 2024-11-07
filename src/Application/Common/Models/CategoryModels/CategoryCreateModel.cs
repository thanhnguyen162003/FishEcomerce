using Domain.Enums;

namespace Application.Common.Models.CategoryModels;

public class CategoryCreateModel
{
    public string? Name { get; set; }
    
    public CategoryType? Type { get; set; }
}