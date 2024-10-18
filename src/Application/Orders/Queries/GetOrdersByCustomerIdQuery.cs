using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Microsoft.Extensions.Options;

namespace Application.Orders.Queries;

public record GetOrdersByCustomerIdQuery : IRequest<PaginatedList<OrderResponseModel>>
{
    public OrderQueryFilter QueryFilter { get; init; }
}

public class GetOrdersByCustomerIdQueryHandler : IRequestHandler<GetOrdersByCustomerIdQuery, PaginatedList<OrderResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;
    private readonly IClaimsService _claimsService;

    public GetOrdersByCustomerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
        _claimsService = claimsService;
    }

    public async Task<PaginatedList<OrderResponseModel>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;

        var customerId = _claimsService.GetCurrentUserId;
        
        var queryable = _unitOfWork.OrderRepository.GetAll()
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.OrderDetails)
            .ThenInclude(y => y.Product)
            .AsQueryable();
        
        queryable = Sort(queryable, request.QueryFilter);
        queryable = queryable.Skip((request.QueryFilter.PageNumber - 1) * request.QueryFilter.PageSize).Take(request.QueryFilter.PageSize);
        var orderList = await queryable.AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        var orders = _mapper.Map<List<OrderResponseModel>>(orderList);
        var count = orders.Count;
        
        return count == 0
            ? new PaginatedList<OrderResponseModel>([], 0, 0, 0)
            : new PaginatedList<OrderResponseModel>(orders, count, request.QueryFilter.PageNumber,
                request.QueryFilter.PageSize);
    }
    
    private IQueryable<Domain.Entites.Order> Sort(IQueryable<Domain.Entites.Order> queryable, OrderQueryFilter orderQueryFilter)
    {
        queryable = orderQueryFilter.Direction.ToLower() switch
        {
            "desc" =>  queryable.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id),
            _ => queryable.OrderBy(x => x.CreatedAt).ThenByDescending(x => x.Id)
                
        };
        return queryable;
    }
}