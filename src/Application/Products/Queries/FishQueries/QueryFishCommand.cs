using System.Linq;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Constants;
using Domain.Entites;
namespace Application.Products.Queries.FishQueries;
#pragma warning disable
public record QueryFishCommand : IRequest<PaginatedList<FishResponseModel>>
{
    public FishQueryFilter QueryFilter;
}

public class QueryFishCommandHandler : IRequestHandler<QueryFishCommand, PaginatedList<FishResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryFishCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FishResponseModel>> Handle(QueryFishCommand request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber == 0 ? 1 : request.QueryFilter.PageNumber; //default page = 1
        request.QueryFilter.PageSize = request.QueryFilter.PageSize == 0 ? 12 : request.QueryFilter.PageSize; //default page size = 12
        request.QueryFilter.Search = request.QueryFilter.Search == null ? "" : request.QueryFilter.Search;

        var queryable = _unitOfWork.FishRepository.GetAll()
            .Include(x => x.Breed)
            .Include(x => x.Product)
            .Include(x => x.Awards)
            .AsQueryable();

        queryable = Filter(queryable, request.QueryFilter);
        queryable = Sort(queryable, request.QueryFilter);
        queryable = queryable.Skip((request.QueryFilter.PageNumber - 1) * request.QueryFilter.PageSize).Take(request.QueryFilter.PageSize);
        var productList = await queryable.AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        var products = _mapper.Map<List<FishResponseModel>>(productList);
        var count = productList.Count;

        if (!queryable.Any())
        {
            return new PaginatedList<FishResponseModel>(new List<FishResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<FishResponseModel>>(queryable);
        return PaginatedList<FishResponseModel>.Create(mapperList, request.QueryFilter.PageNumber, request.QueryFilter.PageSize);
    }
    private IQueryable<Fish> Filter(IQueryable<Fish> queryable, FishQueryFilter fishQueryFilter)
    {
        if (!string.IsNullOrEmpty(fishQueryFilter.Search))
        {
            queryable = queryable.Where(p => p.Product.Name.Contains(fishQueryFilter.Search));
        }

        if (!string.IsNullOrEmpty(fishQueryFilter.Breed))
        {
            queryable = queryable.Where(p => p.Breed.Name.Contains(fishQueryFilter.Breed));
        }

        return queryable;
    }

    private IQueryable<Fish> Sort(IQueryable<Fish> queryable, FishQueryFilter fishQueryFilter)
    {
        queryable = fishQueryFilter.Sort.ToLower() switch
        {
            "price" => fishQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.Product.Price).ThenByDescending(x => x.Product.CreatedAt)
                : queryable.OrderBy(x => x.Product.Price).ThenByDescending(x => x.Product.CreatedAt),
            _ => fishQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.Product.CreatedAt)
                : queryable.OrderBy(x => x.Product.CreatedAt)
        };
        return queryable;
    }
}