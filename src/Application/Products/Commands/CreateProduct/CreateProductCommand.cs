using System.Net;
using FishEcomerce.Application.Common.Models;
using FishEcomerce.Application.Common.Models.ProductModels;
using FishEcomerce.Domain.Entities;
using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection.Common.Utils;

namespace Microsoft.Extensions.DependencyInjection.Products.Commands;

public record CreateProductCommand : IRequest<ResponseModel>
{
    public required ProductRequestModel Model { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var id = new UuidV7().Value;
        var slug = SlugHelper.GenerateSlug(request.Model.Name, id.ToString());

        var product = _mapper.Map<Product>(request.Model);
        product.Id = id;
        product.Slug = slug;
        product.CreatedAt = DateTime.Now;
        
        await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result > 0
            ? new ResponseModel(HttpStatusCode.Created, "Create product successfully.", product.Id)
            : new ResponseModel(HttpStatusCode.BadRequest, "Create product failed.");
    }
}
