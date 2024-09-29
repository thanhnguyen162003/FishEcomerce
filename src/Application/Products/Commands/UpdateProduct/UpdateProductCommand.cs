using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;

namespace Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
    public UpdateTankProductModel UpdateTankProductModel { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<ResponseModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }
        
        product.Name = request.UpdateTankProductModel.Name ?? product.Name;
        product.Description = request.UpdateTankProductModel.Description ?? product.Description;
        product.DescriptionDetail = request.UpdateTankProductModel.DescriptionDetail ?? product.DescriptionDetail;
        product.StockQuantity = request.UpdateTankProductModel.StockQuantity ?? product.StockQuantity;
        product.Price = request.UpdateTankProductModel.Price ?? product.Price;
        product.OriginalPrice = request.UpdateTankProductModel.OriginalPrice ?? product.OriginalPrice;
        
        return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
    }
}
