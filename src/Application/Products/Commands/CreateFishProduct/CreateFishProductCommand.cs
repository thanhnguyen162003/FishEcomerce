using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
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

    public CreateFishProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        //// not have supperId yet
        //// not add images yet


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
        // not add category yet

        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.FishRepository.AddAsync(fish, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 1)
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
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
    }
}