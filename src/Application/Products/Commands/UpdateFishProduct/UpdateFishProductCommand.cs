using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
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
        var product = await _unitOfWork.ProductRepository.GetProductIncludeFishById(request.ProductId);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }
        
        var check = await _unitOfWork.BreedRepository.GetByIdAsync(request.FishProductUpdateModel.FishModel.BreedId);
        
        if (check == null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Breed not found.");
        }
        
        // product
        product.Name = request.FishProductUpdateModel.Name ?? product.Name;
        product.Description = request.FishProductUpdateModel.Description ?? product.Description;
        product.DescriptionDetail = request.FishProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
        product.StockQuantity = request.FishProductUpdateModel.StockQuantity ?? product.StockQuantity;
        product.Price = request.FishProductUpdateModel.Price ?? product.Price;
        product.OriginalPrice = request.FishProductUpdateModel.OriginalPrice ?? product.OriginalPrice;
        product.Type = TypeConstant.FISH;

        List<FishAward> fishAwardsUpdate = new List<FishAward>();
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
            product.Fish.Sex = request.FishProductUpdateModel.FishModel.Sex ? "male" : "female";
            product.UpdatedAt = DateTime.Now;

            //awards
            if (request.FishProductUpdateModel.FishModel.DeleteAward.Any())
            {
                var deleteAwards =
                    await _unitOfWork.FishAwardRepository.GetAwardByIdAsync(request.FishProductUpdateModel.FishModel
                        .DeleteAward);
                foreach (var award in deleteAwards)
                {
                    product.Fish.Awards.Remove(award);
                }
            }

            if (request.FishProductUpdateModel.FishModel.FishAward != null && request.FishProductUpdateModel.FishModel.FishAward.Any())
            {
                
                foreach (var item in request.FishProductUpdateModel.FishModel.FishAward)
                {
                    var award =
                    await _unitOfWork.FishAwardRepository.GetByIdAsync(item.Id);
                    if (award is null)
                    {
                        var create = new FishAward()
                        {
                            Id = new UuidV7().Value,
                            Name = item.Name,
                            FishId = product.Fish.Id,
                            Description = item.Description,
                            AwardDate = DateOnly.FromDateTime(item.AwardDate),
                        };
                        product.Fish.Awards.Add(create);
                    }
                    else
                    {
                        award.Name = item.Name ?? award.Name;
                        award.Description = item.Description ?? award.Description;
                        award.AwardDate = DateOnly.FromDateTime(item.AwardDate);
                        fishAwardsUpdate.Add(award);
                    }
                }

            }

        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.ProductRepository.Update(product);
            if (fishAwardsUpdate.Count() > 0)
            {
                _unitOfWork.FishAwardRepository.UpdateRange(fishAwardsUpdate);
            }
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Update fish product successfully.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update fish product failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
            
    }
}