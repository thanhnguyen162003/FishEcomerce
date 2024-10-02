using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
namespace Application.Products.Queries.FishQueries;
#pragma warning disable
public record QueryFishProductCommand : IRequest<PaginatedList<ProductResponseModel>>
{
    public FishQueryFilter QueryFilter;
}

public class QueryFishProductCommandHandler : IRequestHandler<QueryFishProductCommand, PaginatedList<ProductResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryFishProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductResponseModel>> Handle(QueryFishProductCommand request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber == 0 ? 1 : request.QueryFilter.PageNumber; //default page = 1
        request.QueryFilter.PageSize = request.QueryFilter.PageSize == 0 ? 12 : request.QueryFilter.PageSize; //default page size = 12
        request.QueryFilter.Search = request.QueryFilter.Search == null ? "" : request.QueryFilter.Search;
        var breedList = await _unitOfWork.ProductRepository.GetAllProductIncludeFish();

        if (!breedList.Any())
        {
            return new PaginatedList<ProductResponseModel>(new List<ProductResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<ProductResponseModel>>(breedList);
        return PaginatedList<ProductResponseModel>.Create(mapperList, request.QueryFilter.PageNumber, request.QueryFilter.PageSize);
    }
}