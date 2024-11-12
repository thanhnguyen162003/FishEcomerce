using System.Text.RegularExpressions;
using Application.Common.Models;
using Application.Common.Models.StaffModels;
using Application.Common.UoW;
using Domain.Entites;
using Microsoft.Extensions.Options;

namespace Application.Staffs.Queries;

public record GetStaffsQuery : IRequest<PaginatedList<StaffResponseModel>>
{
    public StaffQueryFilter Filter { get; init; }
}

public class GetStaffsQueryHandler : IRequestHandler<GetStaffsQuery, PaginatedList<StaffResponseModel>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PaginationOptions _paginationOptions;

    public GetStaffsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PaginatedList<StaffResponseModel>> Handle(GetStaffsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.StaffRepository.GetAll().Where(x => x.DeletedAt == null).AsNoTracking();
        queryable = Filter(queryable, request.Filter);
        queryable = Sort(queryable, request.Filter);
        var count = await queryable.CountAsync(cancellationToken);
        
        if (request.Filter.PageNumber is not null && request.Filter.PageSize is not null)
        {
            request.Filter.PageSize = request.Filter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.Filter.PageSize;
            request.Filter.PageNumber = request.Filter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.Filter.PageNumber;
            queryable = queryable.Skip(((int)request.Filter.PageNumber - 1) * (int)request.Filter.PageSize).Take((int)request.Filter.PageSize);
        }

        
        var staffs = await queryable.ToListAsync(cancellationToken);
        var mapper = _mapper.Map<List<StaffResponseModel>>(staffs);
        
        return count == 0 
            ? new PaginatedList<StaffResponseModel>([],0,0,0)
            : new PaginatedList<StaffResponseModel>(mapper, count, request.Filter.PageNumber ?? 0, request.Filter.PageSize ?? 0);
    }
    
    private IQueryable<Staff> Filter(IQueryable<Staff> queryable, StaffQueryFilter customerQueryFilter)
    {
        if (!string.IsNullOrEmpty(customerQueryFilter.Search))
        {
            var normalInput = Regex.Replace(customerQueryFilter.Search.Trim(), @"\s+", " ").ToLower();
            queryable = queryable.Where(p => p.FullName!.ToLower().Contains(normalInput));
        }
        
        return queryable;
    }
    
    private IQueryable<Staff> Sort(IQueryable<Staff> queryable, StaffQueryFilter customerQueryFilter)
    {
        queryable = customerQueryFilter.Sort!.ToLower() switch
        {
            "date" => customerQueryFilter.Direction!.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
                : queryable.OrderBy(x => x.CreatedAt).ThenByDescending(x => x.Id),
            _ => customerQueryFilter.Direction!.ToLower() == "desc"
                ? queryable.OrderByDescending(x => x.FullName).ThenByDescending(x => x.Id)
                : queryable.OrderBy(x => x.FullName).ThenByDescending(x => x.Id)
        };
        return queryable;
    }
}