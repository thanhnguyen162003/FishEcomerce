using Microsoft.AspNetCore.Http;

namespace Application.Common.Models.ImageModels;

public class ImageUploadRequestModel
{
    public IFormFile File { get; set; }
}