using Application.Common.Models.CategoryModels;

namespace Application.Common.Models.TankModels;

public class TankResponseModel
{
    public Guid Id { get; set; }
    
    public Guid? ProductId { get; set; }

    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }
    
    public IEnumerable<CategoryResponseModel> Categories { get; set; }

}