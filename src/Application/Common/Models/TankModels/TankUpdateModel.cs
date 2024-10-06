namespace Application.Common.Models.TankModels;

public class TankUpdateModel
{
    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }
    
    public IEnumerable<Guid> DeleteCategories { get; set; } = new List<Guid>();
    
    public IEnumerable<Guid> UpdateCategories { get; set; } = new List<Guid>();

}