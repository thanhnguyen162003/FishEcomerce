namespace Application.Common.Models.ImageModels;

public class ImageResponseModel
{
    public Guid Id { get; set; }
    
    public string? PublicId { get; set; }
    
    public string? Link { get; set; }

    public Guid? ProductId { get; set; }
}