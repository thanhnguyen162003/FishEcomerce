using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Common.ThirdPartyManager.Cloudinary;

public interface ICloudinaryService
{
    Task<ImageUploadResult> UploadAsync(IFormFile file);
    Task DeleteAsync(string publicId);
}

public class CloudinaryService : ICloudinaryService
{
    
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> configuration)
    {
        var account = new Account(
            configuration.Value.CloudName,
            configuration.Value.ApiKey,
            configuration.Value.ApiSecret);
        
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
    }
    
    
    public async Task<ImageUploadResult> UploadAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream)
        };

        var result = await _cloudinary.UploadAsync(uploadParams); 
        return result;
    }

    public async Task DeleteAsync(string publicId)
    {
        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}