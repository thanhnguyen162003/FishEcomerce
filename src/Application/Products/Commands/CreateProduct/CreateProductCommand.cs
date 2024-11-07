
using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<ResponseModel>
{
    public ProductCreateModel ProductCreateModel { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;
    private readonly ICloudinaryService _cloudinaryService;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var categories =
            await _unitOfWork.CategoryRepository.GetCategoriesByIdAsync(request.ProductCreateModel.CategoriesIds);
            
        if (!categories.Any())
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "No categories found");
        }    
            
        var staffId = _claimsService.GetCurrentUserId;
        var product = _mapper.Map<Product>(request.ProductCreateModel);
        product.Id = new UuidV7().Value;
        product.Slug = SlugHelper.GenerateSlug(product.Name, product.Id.ToString());
        product.CreatedAt = DateTime.Now;
        product.UpdatedAt = DateTime.Now;
        product.StaffId = staffId;
        product.Categories = categories;
        
        var errors = 0;
        foreach (var file in request.ProductCreateModel.ImageFiles)
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

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            var data = "";
            if (errors != 0)
            {
                data = "Some images could not be saved.";
            }
            
            return new ResponseModel(HttpStatusCode.OK, "Create product successfully.", new {errors = data});
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