using System.Net;
using Application.Common.Models;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;
using Microsoft.AspNetCore.Http;

namespace Application.Images.Commands;

public record UpdateImageCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public IEnumerable<IFormFile> UpdateImages { get; init; }
    public IEnumerable<Guid> DeleteImages { get; init; }
}

public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudinaryService _cloudinaryService;

    public UpdateImageCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResponseModel> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductIncludeImageById(request.ProductId);
        
        if (request.DeleteImages.Any())
        {
            var deleteImages =
                await _unitOfWork.ImageRepository.GetImagesByIdAsync(request.DeleteImages);
            foreach (var image in deleteImages)
            {
                product.Images.Remove(image);
            }
        }
        
        // image
        var errors = 0;
        var images = new List<Image>();
        if (request.UpdateImages.Any())
        {
            foreach (var file in request.UpdateImages)
            {
                var upload = await _cloudinaryService.UploadAsync(file);
                if (upload.Error is not null)
                {
                    errors++;
                    continue;
                }

                var image = new Image()
                {
                    Id = new UuidV7().Value,
                    ProductId = product.Id,
                    PublicId = upload.PublicId,
                    Link = upload.Url.ToString()
                };

                images.Add(image);
            }

            foreach (var image in images)
            {
                product.Images.Add(image);
            }
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ImageRepository.AddRangeAsync(images, cancellationToken);
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync();
            var data = "";
            if (errors != 0)
            {
                data = "Some images could not be saved.";
            }
            
            return new ResponseModel(HttpStatusCode.OK, "Update product successfully.", data);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            foreach (var entity in images)
            {
                await _cloudinaryService.DeleteAsync(entity.PublicId);
            }
            throw;
        }
    }
}