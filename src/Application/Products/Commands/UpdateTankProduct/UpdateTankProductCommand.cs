using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;

namespace Application.Products.Commands.UpdateProduct;

public record UpdateTankProductCommand : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
    public UpdateTankProductModel UpdateTankProductModel { get; init; }
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
        var product = await _unitOfWork.ProductRepository.GetProductIncludeTankById(request.Id);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // product
            product.Name = request.UpdateTankProductModel.Name ?? product.Name;
            product.Description = request.UpdateTankProductModel.Description ?? product.Description;
            product.DescriptionDetail = request.UpdateTankProductModel.DescriptionDetail ?? product.DescriptionDetail;
            product.StockQuantity = request.UpdateTankProductModel.StockQuantity ?? product.StockQuantity;
            product.Price = request.UpdateTankProductModel.Price ?? product.Price;
            product.OriginalPrice = request.UpdateTankProductModel.OriginalPrice ?? product.OriginalPrice;
            _unitOfWork.ProductRepository.Update(product);
            
            // tank
            if (request.UpdateTankProductModel.TankModel is not null)
            {
                product.Tank.Size = request.UpdateTankProductModel.TankModel.Size ?? product.Tank.Size;
                product.Tank.SizeInformation = request.UpdateTankProductModel.TankModel.SizeInformation ??
                                               product.Tank.SizeInformation;
                product.Tank.GlassType = request.UpdateTankProductModel.TankModel.GlassType ?? product.Tank.GlassType;
                _unitOfWork.TankRepository.Update(product.Tank);
            }
            
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Update tank product successfully.", product.Id);
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update tank product failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update tank product failed.", e.Message);
        }

    }
}