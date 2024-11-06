using System.Net;
using Application.Common.Models;
using Application.Common.Models.ImageModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;
using Microsoft.AspNetCore.Http;

namespace Application.Images.Commands.UploadImage;

public record UploadImageCommand : IRequest<ResponseModel>
{
    public ImageUploadRequestModel ImageUploadRequestModel { get; init; }
}

public class UpdateBlogImageCommandHandler : IRequestHandler<UploadImageCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudinaryService _cloudinaryService;

    public UpdateBlogImageCommandHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResponseModel> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var upload = await _cloudinaryService.UploadAsync(request.ImageUploadRequestModel.File);
        
        if (upload.Error is not null)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, upload.Error.Message);
        }

        var image = new Image
        {
            Id = new UuidV7().Value,
            PublicId = upload.PublicId,
            Link = upload.Url.ToString()
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ImageRepository.AddAsync(image, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Image successfully uploaded", new {imageLink = image.Link});
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}