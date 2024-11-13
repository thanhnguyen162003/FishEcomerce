using System.Text.RegularExpressions;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Constants;
using Domain.Entites;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Application.Products.Queries.ProductQueries;

public record GetProductsQuery : IRequest<PaginatedList<ProductResponseModel>>
{
    public CategoryType Type { get; init; }
    public ProductQueryFilter QueryFilter { get; init; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;
    private readonly IClaimsService _claimsService;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
        _claimsService = claimsService;
    }

    public async Task<PaginatedList<ProductResponseModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.ProductRepository.GetAll()
            .Where(x => x.Type.Equals(request.Type.ToString().ToLower()) && x.DeletedAt == null)
            .Include(x => x.Images)
            .Include(x => x.Feedbacks)
            .Include(x => x.Staff)
            .Include(x => x.Categories)
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
    
    private IQueryable<Product> Filter(IQueryable<Product> queryable, ProductQueryFilter productQueryFilter)
    {
        if (productQueryFilter.PriceFrom.HasValue)
        {
            queryable = queryable.Where(p => p.Price >= productQueryFilter.PriceFrom);
        }
        
        if (productQueryFilter.PriceTo.HasValue)
        {
            queryable = queryable.Where(p => p.Price <= productQueryFilter.PriceTo);
        }
        
        if (!string.IsNullOrEmpty(productQueryFilter.Search))
        {
            var normalInput = Regex.Replace(productQueryFilter.Search.Trim(), @"\s+", " ").ToLower();
            queryable = queryable.Where(p => p.Name.ToLower().Contains(normalInput));
        }

        if (!string.IsNullOrEmpty(productQueryFilter.Category))
        {
            queryable = queryable.Where(p => p.Categories.Any(x => x.Name.ToLower().Equals(productQueryFilter.Category.ToLower())));
        }
        
        return queryable;
    }
    
    private IQueryable<Product> Sort(IQueryable<Product> queryable, ProductQueryFilter productQueryFilter)
    {
        queryable = productQueryFilter.Sort.ToLower() switch
        {
            "date" => productQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id)
                : queryable.OrderBy(x => x.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id),
            _ => productQueryFilter.Direction.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.Price).ThenByDescending(x => x.CreatedAt).ThenBy(x => x.Id)
                : queryable.OrderBy(x => x.Price).ThenByDescending(x => x.CreatedAt).ThenBy(x => x.Id)
        };
        return queryable;
    }
}