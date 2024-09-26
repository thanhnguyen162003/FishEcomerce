using System.Net;
using FishEcomerce.Application.Common.Models;
using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;

namespace Microsoft.Extensions.DependencyInjection.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<ResponseModel>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.BadGateway, "Product not exist to delete.");
        }
        
        product.DeletedAt = DateTime.Now;

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result > 0
            ? new ResponseModel(HttpStatusCode.OK, "Delete product successfully.")
            : new ResponseModel(HttpStatusCode.BadRequest, "Delete product failed.");
    }
}
