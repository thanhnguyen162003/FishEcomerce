using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.CustomerFeature.Queries;

public record GetCustomersQuery : IRequest<PaginatedList<CustomerResponseModel>>
{
    public CustomerQueryFilter QueryFilter { get; init; }
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerResponseModel>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PaginationOptions _paginationOptions;

    public GetCustomersQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PaginatedList<CustomerResponseModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var queryable = _mapper.ProjectTo<CustomerResponseModel>(_unitOfWork.CustomerRepository.GetAll().AsNoTracking());
        
        if (request.QueryFilter.PageNumber is not null && request.QueryFilter.PageSize is not null)
        {
            request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }
        
        queryable = Filter(queryable, request.QueryFilter);
        queryable = Sort(queryable, request.QueryFilter);
        var customers = await queryable.ToListAsync(cancellationToken);
        var count = await queryable.CountAsync(cancellationToken);

        return count == 0
            ? new PaginatedList<CustomerResponseModel>([], 0, 0, 0)
            : new PaginatedList<CustomerResponseModel>(customers, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }
    
    private IQueryable<CustomerResponseModel> Filter(IQueryable<CustomerResponseModel> queryable, CustomerQueryFilter customerQueryFilter)
    {
        if (!string.IsNullOrEmpty(customerQueryFilter.Search))
        {
            queryable = queryable.Where(p => p.Name!.ToLower().Contains(customerQueryFilter.Search.ToLower()));
        }
        
        return queryable;
    }
    
    private IQueryable<CustomerResponseModel> Sort(IQueryable<CustomerResponseModel> queryable, CustomerQueryFilter customerQueryFilter)
    {
        queryable = customerQueryFilter.Sort!.ToLower() switch
        {
            "date" => customerQueryFilter.Direction!.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.RegistrationDate).ThenByDescending(x => x.Id)
                : queryable.OrderBy(x => x.RegistrationDate).ThenByDescending(x => x.Id),
            _ => customerQueryFilter.Direction!.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id)
                : queryable.OrderBy(x => x.Name).ThenByDescending(x => x.Id)
        };
        return queryable;
    }
}