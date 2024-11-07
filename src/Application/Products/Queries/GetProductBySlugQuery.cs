using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Constants;
using Domain.Entites;

namespace Application.Products.Queries;

public record GetProductBySlugQuery : IRequest<ResponseModel>
{
    public string Slug { get; init; }
}

public class GetProductBySlugQueryHandler : IRequestHandler<GetProductBySlugQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAll().FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found.");
        }
        if(product.Type == TypeConstant.FISH ){
            product = await _unitOfWork.ProductRepository.GetAll()
            .Include(x => x.Fish)
            .Include(x => x.Fish.Breed)
            .Include(x => x.Fish.Awards)
            .Include(x => x.Images)
            .Include(x => x.Staff)
            .Include(x => x.Feedbacks)
            .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);
        }

        if (product.Type == TypeConstant.TANK)
        {
            product = await _unitOfWork.ProductRepository.GetAll()
            .Include(x => x.Tank)
            .Include(x => x.Tank.TankCategories)
            .Include(x => x.Images)
            .Include(x => x.Staff)
            .Include(x => x.Feedbacks)
            .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);
        }

        var mapper = _mapper.Map<ProductResponseModel>(product);
        return new ResponseModel(HttpStatusCode.OK,"", mapper);
    }
}