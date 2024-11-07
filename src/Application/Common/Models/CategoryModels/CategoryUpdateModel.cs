using Domain.Enums;

namespace Application.Common.Models.CategoryModels;

public class CategoryUpdateModel
{
    public string? Name { get; set; }
    
    public CategoryType? Type { get; set; }
}