namespace Application.Common.Models.CategoryModels;

public class CategoryResponseModel
{
    public Guid Id { get; set; }
    
    public string? TankType { get; set; }

    public string? Level { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}