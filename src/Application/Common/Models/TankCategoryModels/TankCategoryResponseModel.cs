namespace Application.Common.Models.TankCategoryModels;

public class TankCategoryResponseModel
{
    public Guid Id { get; set; }
    
    public string? TankType { get; set; }

    public string? Level { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}