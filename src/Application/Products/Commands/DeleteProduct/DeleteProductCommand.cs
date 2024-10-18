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
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Product has been deleted.");
            
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
        throw;
        }
        
    }
}