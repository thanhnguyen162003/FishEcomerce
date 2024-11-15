using System.Text.RegularExpressions;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Constants;
using Domain.Entites;
using Microsoft.Extensions.Options;

namespace Application.Products.Queries.TankQueries;

public record GetTankWithPaginationQuery : IRequest<PaginatedList<ProductResponseModel>>
{
    public TankQueryFilter QueryFilter { get; init; }
}

public class GetTankWithPaginationQueryHandler : IRequestHandler<GetTankWithPaginationQuery, PaginatedList<ProductResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;
    private readonly IClaimsService _claimsService;

    public GetTankWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
        _claimsService = claimsService;
    }

    public async Task<PaginatedList<ProductResponseModel>> Handle(GetTankWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.ProductRepository.GetAll()
            .Where(x => x.Type.Equals(TypeConstant.TANK) && x.DeletedAt == null)
            .Include(x => x.Tank)
            .ThenInclude(x => x.TankCategories.Where(tc => tc.DeletedAt == null))
            .Include(x => x.Images)
            .Include(x => x.Feedbacks)
            .Include(x => x.Staff)
            .AsNoTracking()
            .AsQueryable();
        
        if (string.IsNullOrEmpty(_claimsService.GetCurrentRole) || _claimsService.GetCurrentRole.Equals("Customer"))
        {
            queryable = queryable.Where(x => x.StockQuantity > 0);
        }
        
        queryable = Filter(queryable, request.QueryFilter);
        queryable = Sort(queryable, request.QueryFilter);
        var count = await queryable.CountAsync(cancellationToken);

        if (request.QueryFilter.PageSize is not null && request.QueryFilter.PageNumber is not null)
        {
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            request.QueryFilter.PageSize = request.QueryFilter.PageSize == 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }
        
        var productList = await queryable.AsSplitQuery().ToListAsync(cancellationToken);
        var products = _mapper.Map<List<ProductResponseModel>>(productList);
        return count == 0
            ? new PaginatedList<ProductResponseModel>([], 0, 0, 0)
            : new PaginatedList<ProductResponseModel>(products, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }

    private IQueryable<Product> Filter(IQueryable<Product> queryable, TankQueryFilter tankQueryFilter)
    {
        if (tankQueryFilter.PriceFrom.HasValue)
        {
            queryable = queryable.Where(p => p.Price >= tankQueryFilter.PriceFrom);
        }
        
        if (tankQueryFilter.PriceTo.HasValue)
        {
            queryable = queryable.Where(p => p.Price <= tankQueryFilter.PriceTo);
        }
        
        if (!string.IsNullOrEmpty(tankQueryFilter.Search))
        {
            var normalInput = Regex.Replace(tankQueryFilter.Search.Trim(), @"\s+", " ").ToLower();
            queryable = queryable.Where(p => p.Name.ToLower().Contains(normalInput));
        }

        if (!string.IsNullOrEmpty(tankQueryFilter.Category))
        {
            queryable = queryable.Where(p => p.Tank.TankCategories.Any(x => x.TankType.ToLower().Equals(tankQueryFilter.Category.ToLower())));
        }
        
        return queryable;
    }

    private IQueryable<Product> Sort(IQueryable<Product> queryable, TankQueryFilter tankQueryFilter)
    {
        queryable = tankQueryFilter.Sort.ToLower() switch
        {
            "date" => tankQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id)
                : queryable.OrderBy(x => x.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id),
            _ => tankQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.Price).ThenByDescending(x => x.CreatedAt).ThenBy(x => x.Id)
                : queryable.OrderBy(x => x.Price).ThenByDescending(x => x.CreatedAt).ThenBy(x => x.Id)
        };
        return queryable;
    }
}

