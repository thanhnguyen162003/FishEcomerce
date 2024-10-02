using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Constants;

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
        var check = _unitOfWork.BreedRepository.GetByIdAsync(request.FishProductUpdateModel.FishModel.BreedId);
        if (check == null)
        {
            return new ResponseModel(HttpStatusCode.BadGateway, "Breed not found.");
        }
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // product
            product.Name = request.FishProductUpdateModel.Name ?? product.Name;
            product.Description = request.FishProductUpdateModel.Description ?? product.Description;
            product.DescriptionDetail = request.FishProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
            product.StockQuantity = request.FishProductUpdateModel.StockQuantity ?? product.StockQuantity;
            product.Price = request.FishProductUpdateModel.Price ?? product.Price;
            product.OriginalPrice = request.FishProductUpdateModel.OriginalPrice ?? product.OriginalPrice;
            product.Type = TypeConstant.FISH;
            _unitOfWork.ProductRepository.Update(product);
            
            //// tank
            //if (request.FishProductUpdateModel.FishModel is not null)
            //{
            //    product.Tank.Size = request.FishProductUpdateModel.TankModel.Size ?? product.Tank.Size;
            //    product.Tank.SizeInformation = request.FishProductUpdateModel.TankModel.SizeInformation ??
            //                                   product.Tank.SizeInformation;
            //    product.Tank.GlassType = request.FishProductUpdateModel.TankModel.GlassType ?? product.Tank.GlassType;
            //    _unitOfWork.TankRepository.Update(product.Tank);
            //}
            
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
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }

    }
}