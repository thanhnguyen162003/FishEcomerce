using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<ResponseModel>
{
    public ProductCreateModel ProductModel { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var id = new UuidV7().Value;
        var slug = SlugHelper.GenerateSlug(request.ProductModel.Name, id.ToString());

        var product = _mapper.Map<Product>(request.ProductModel);
        product.Id = id;
        product.Slug = slug;
        product.CreatedAt = DateTime.Now;
        
        // not add images yet

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create product successfully.", product.Id);
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create product failed.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create product failed.");
        }
    }
}