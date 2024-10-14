using System.Linq;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.UoW;
using Domain.Constants;
using Domain.Entites;
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

        var queryable = await _unitOfWork.ProductRepository.GetAllProductIncludeFish();

        queryable = Filter(queryable, request.QueryFilter);
        queryable = Sort(queryable, request.QueryFilter);
        queryable = queryable.Skip((request.QueryFilter.PageNumber - 1) * request.QueryFilter.PageSize).Take(request.QueryFilter.PageSize);
        var productList = await queryable.AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        var products = _mapper.Map<List<ProductResponseModel>>(productList);
        var count = productList.Count;

        if (!queryable.Any())
        {
            return new PaginatedList<ProductResponseModel>(new List<ProductResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<ProductResponseModel>>(queryable);
        return PaginatedList<ProductResponseModel>.Create(mapperList, request.QueryFilter.PageNumber, request.QueryFilter.PageSize);
    }
    private IQueryable<Product> Filter(IQueryable<Product> queryable, FishQueryFilter fishQueryFilter)
    {
        if (fishQueryFilter.PriceFrom.HasValue)
        {
            queryable = queryable.Where(p => p.Price >= fishQueryFilter.PriceFrom);
        }
        if (fishQueryFilter.PriceTo.HasValue)
        {
            queryable = queryable.Where(p => p.Price <= fishQueryFilter.PriceTo);
        }
        if (!string.IsNullOrEmpty(fishQueryFilter.Search))
        {
            queryable = queryable.Where(p => p.Name.Contains(fishQueryFilter.Search));
        }

        if (!string.IsNullOrEmpty(fishQueryFilter.Breed))
        {
            queryable = queryable.Where(p => p.Fish.Breed.Name.Contains(fishQueryFilter.Breed));
        }

        return queryable;
    }

    private IQueryable<Product> Sort(IQueryable<Product> queryable, FishQueryFilter fishQueryFilter)
    {
        string sort = fishQueryFilter.Sort?.ToLower() ?? "createdat"; // Default to sorting by CreatedAt
        string direction = fishQueryFilter.Direction?.ToLower() ?? "desc"; // Default to descending order

        switch (sort)
        {
            case "price":
                return direction == "desc"
                    ? queryable.OrderByDescending(x => x.Price).ThenByDescending(x => x.CreatedAt)
                    : queryable.OrderBy(x => x.Price).ThenByDescending(x => x.CreatedAt);
            case "createdat":
                return direction == "desc"
                    ? queryable.OrderByDescending(x => x.CreatedAt)
                    : queryable.OrderBy(x => x.CreatedAt);
            default:
                return queryable.OrderByDescending(x => x.CreatedAt); // Default to descending order by CreatedAt
        }

    }
}