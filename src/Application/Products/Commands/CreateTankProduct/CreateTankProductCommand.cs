using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Constants;
using Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Products.Commands.CreateTankProduct;

public record CreateTankProductCommand : IRequest<ResponseModel>
{
    public TankProductCreateModel TankProductCreateModel { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateTankProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;
    private readonly ICloudinaryService _cloudinaryService;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService,
        ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResponseModel> Handle(CreateTankProductCommand request, CancellationToken cancellationToken)
    {
        // category
        var tankCategories =
            await _unitOfWork.TankCategoryRepository.GetTankCategoriesByIdAsync(request.TankProductCreateModel.CategoriesIds);

        if (!tankCategories.Any())
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "No categories found");
        }
        
        // product
        var productId = new UuidV7().Value;
        var slug = SlugHelper.GenerateSlug(request.TankProductCreateModel.Name, productId.ToString());
        var product = _mapper.Map<Product>(request.TankProductCreateModel);
        product.Id = productId;
        product.Slug = slug;
        product.CreatedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;
        product.StaffId = _claimsService.GetCurrentUserId;
        product.Type = TypeConstant.TANK;

        // image
        var errors = 0;
        foreach (var file in request.TankProductCreateModel.ImageFiles)
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
            
            product.Images.Add(image);
        }

        // tank
        var tankId = new UuidV7().Value;
        var tank = _mapper.Map<Tank>(request.TankProductCreateModel.TankModel);
        tank.Id = tankId;
        tank.ProductId = productId;
        tank.TankCategories = tankCategories;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.TankRepository.AddAsync(tank, cancellationToken);
            var check = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (check == 0)
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
            else
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            var data = "";
            if (errors != 0)
            {
                data = "Some images could not be saved.";
            }
            
            return new ResponseModel(HttpStatusCode.OK, "Create tank product successfully.", new {errors = data});
            
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            foreach (var entity in product.Images)
            {
                await _cloudinaryService.DeleteAsync(entity.PublicId);
            }

            throw;
        }
    }
}