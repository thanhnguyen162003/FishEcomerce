using Application.Common.Models.TankCategoryModels;

namespace Application.Common.Models.TankModels;

public class TankResponseModel
{
    public Guid Id { get; set; }
    
    public Guid? ProductId { get; set; }

    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }
    
    public IEnumerable<TankCategoryResponseModel> Categories { get; set; }

}