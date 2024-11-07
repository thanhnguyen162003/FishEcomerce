namespace Application.Common.Models.CategoryModels;

public class CategoryResponseModel
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}