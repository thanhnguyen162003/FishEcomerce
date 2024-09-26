using System.Net;
using FishEcomerce.Application.Common.Models;
using FishEcomerce.Application.Common.Models.ProductModels;
using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection.Common.Utils;

namespace Microsoft.Extensions.DependencyInjection.Products.Commands;

public record UpdateProductCommand : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
    public required ProductUpdateRequestModel Model { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id , cancellationToken);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "product not exist to update.");
        }

        if (request.Model.Name is not null)
        {
            product.Name = request.Model.Name ?? product.Name;
            product.Slug = SlugHelper.GenerateSlug(product.Name!, product.Id.ToString());
        }
        product.Description = request.Model.Description ?? product.Description;
        product.StockQuantity = request.Model.StockQuantity ?? product.StockQuantity;
        product.Price = request.Model.Price ?? product.Price;
        product.UpdatedAt = DateTime.Now;

        _unitOfWork.ProductRepository.Update(product);
        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result > 0
            ? new ResponseModel(HttpStatusCode.OK, "Update product successfully.")
            : new ResponseModel(HttpStatusCode.BadRequest, "Update product failed.");
    }
}
