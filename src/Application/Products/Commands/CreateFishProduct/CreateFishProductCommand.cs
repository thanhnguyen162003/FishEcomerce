using System.Collections.Generic;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using AutoMapper;
using Domain.Constants;
using Domain.Entites;

namespace Application.Products.Commands.CreateFishProduct;

public record CreateFishProductCommand : IRequest<ResponseModel>
{
    public FishProductCreateModel FishProductCreateModel { get; init; }
    public Guid Id { get; init; }
}

public class CreateFishProductCommandHandler : IRequestHandler<CreateFishProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;
    private readonly ICloudinaryService _cloudinaryService;

    public CreateFishProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cloudinaryService = cloudinaryService;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(CreateFishProductCommand request, CancellationToken cancellationToken)
    {
        //// product
        var productId = new UuidV7().Value;
        var slug = SlugHelper.GenerateSlug(request.FishProductCreateModel.Name, productId.ToString());
        var product = _mapper.Map<Product>(request.FishProductCreateModel);
        product.Id = productId;
        product.Slug = slug;
        product.CreatedAt = DateTime.Now;
        product.Type = TypeConstant.FISH;
        product.SupplierId = _claimsService.GetCurrentUserId;

        // image
        var images = new List<Image>();
        var errors = 0;
        foreach (var file in request.FishProductCreateModel.ImageFiles)
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


        // fish
        var check = await _unitOfWork.BreedRepository.GetBreedById(request.FishProductCreateModel.FishModel.BreedId);
        if (check.Count() == 0)
        {
            return new ResponseModel(HttpStatusCode.BadGateway, "Breed not found.");
        }
        var fishId = new UuidV7().Value;
        var fish = _mapper.Map<Fish>(request.FishProductCreateModel.FishModel);
        fish.Id = fishId;
        fish.ProductId = product.Id;
        if (request.FishProductCreateModel.FishModel.Sex)
        {
            fish.Sex = "male";
        }
        else fish.Sex = "female";
        if (request.FishProductCreateModel.FishModel.DateOfBirth.HasValue)
        {
            fish.DateOfBirth = DateOnly.FromDateTime(request.FishProductCreateModel.FishModel.DateOfBirth.Value);
        }
        
        fish.Breed = check.FirstOrDefault();
        var listAward = new List<FishAward>();
        //award
        foreach (var item in request.FishProductCreateModel.FishAward)
        {
            var award = new FishAward() 
            { 
                Id = new UuidV7().Value,
                Name = item.Name,
                FishId = fishId,
                Description = item.Description,
                AwardDate = DateOnly.FromDateTime(item.AwardDate),
            };
            listAward.Add(award);
        }        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.ImageRepository.AddRangeAsync(images, cancellationToken);
            await _unitOfWork.FishAwardRepository.AddRangeAsync(listAward, cancellationToken);
            await _unitOfWork.FishRepository.AddAsync(fish, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            var data = "";
            if (errors != 0)
            {
                data = "some images could not be saved.";
            }
            if (result > 3 && errors != 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create fish successfully. But" + data);
            }
            else if (result > 3 && errors == 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create fish successfully.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create fish failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            foreach (var entity in images)
            {
                await _cloudinaryService.DeleteAsync(entity.PublicId);
            }
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
    }
}