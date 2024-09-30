using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;

namespace Application.Products.Commands.UpdateTankProduct;

public record UpdateTankProductCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public TankProductUpdateModel TankProductUpdateModel { get; init; }
}

public class UpdateTankProductCommandHandler : IRequestHandler<UpdateTankProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTankProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateTankProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductIncludeTankById(request.ProductId);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // product
            product.Name = request.TankProductUpdateModel.Name ?? product.Name;
            product.Description = request.TankProductUpdateModel.Description ?? product.Description;
            product.DescriptionDetail = request.TankProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
            product.StockQuantity = request.TankProductUpdateModel.StockQuantity ?? product.StockQuantity;
            product.Price = request.TankProductUpdateModel.Price ?? product.Price;
            product.OriginalPrice = request.TankProductUpdateModel.OriginalPrice ?? product.OriginalPrice;
            _unitOfWork.ProductRepository.Update(product);
            
            // tank
            if (request.TankProductUpdateModel.TankModel is not null)
            {
                product.Tank.Size = request.TankProductUpdateModel.TankModel.Size ?? product.Tank.Size;
                product.Tank.SizeInformation = request.TankProductUpdateModel.TankModel.SizeInformation ??
                                               product.Tank.SizeInformation;
                product.Tank.GlassType = request.TankProductUpdateModel.TankModel.GlassType ?? product.Tank.GlassType;
                _unitOfWork.TankRepository.Update(product.Tank);
            }
            
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