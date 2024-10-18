using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Constants;
using Domain.Entites;

namespace Application.Products.Commands.UpdateFishProduct;

public record UpdateFishProductCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public FishProductUpdateModel FishProductUpdateModel { get; init; }
}

public class UpdateFishProductCommandHandler : IRequestHandler<UpdateFishProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFishProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateFishProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductIncludeTankById(request.ProductId);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }
        var check = await _unitOfWork.BreedRepository.GetByIdAsync(request.FishProductUpdateModel.FishModel.BreedId);
        if (check == null)
        {
            return new ResponseModel(HttpStatusCode.BadGateway, "Breed not found.");
        }
        
        // product
        product.Name = request.FishProductUpdateModel.Name ?? product.Name;
        product.Description = request.FishProductUpdateModel.Description ?? product.Description;
        product.DescriptionDetail = request.FishProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
        product.StockQuantity = request.FishProductUpdateModel.StockQuantity ?? product.StockQuantity;
        product.Price = request.FishProductUpdateModel.Price ?? product.Price;
        product.OriginalPrice = request.FishProductUpdateModel.OriginalPrice ?? product.OriginalPrice;
        product.Type = TypeConstant.FISH;


        // fish
        if (request.FishProductUpdateModel.FishModel is not null)
        {
            product.Fish.Size = request.FishProductUpdateModel.FishModel.Size.ToString() ?? product.Fish.Size;
            product.Fish.BreedId = request.FishProductUpdateModel.FishModel.BreedId ?? product.Fish.BreedId;
            product.Fish.Age = request.FishProductUpdateModel.FishModel.Age.ToString() ?? product.Fish.Age;
            product.Fish.FoodAmount = request.FishProductUpdateModel.FishModel.FoodAmount.ToString() ?? product.Fish.FoodAmount;
            product.Fish.Weight = request.FishProductUpdateModel.FishModel.Weight ?? product.Fish.Weight;
            product.Fish.Origin = request.FishProductUpdateModel.FishModel.Origin ?? product.Fish.Origin;
            product.Fish.Health = request.FishProductUpdateModel.FishModel.Health ?? product.Fish.Health;
            if (request.FishProductUpdateModel.FishModel.Sex)
            {
                product.Fish.Sex = "male";
            }
            else product.Fish.Sex = "female";
            product.UpdatedAt = DateTime.Now;

        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.ProductRepository.Update(product);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Update tank product successfully.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update tank product failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
            
    }
}