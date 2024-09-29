using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseModel> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Product not found.");
        }
        
        product.DeletedAt = DateTime.Now;
        _unitOfWork.ProductRepository.Update(product);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result > 0
            ? new ResponseModel(HttpStatusCode.OK, "Delete product successfully.")
            : new ResponseModel(HttpStatusCode.BadRequest, "Delete product failed.");
    }
}