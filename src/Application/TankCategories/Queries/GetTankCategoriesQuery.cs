using Application.Common.Models;
using Application.Common.Models.TankCategoryModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.Categories.Queries;

public record GetTankCategoriesQuery : IRequest<PaginatedList<TankCategoryResponseModel>>
{
    public TankCategoryQueryFilter QueryFilter { get; init; }
}

public class GetCategoriesWithPaginationHandler : IRequestHandler<GetTankCategoriesQuery, PaginatedList<TankCategoryResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;

    public GetCategoriesWithPaginationHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PaginatedList<TankCategoryResponseModel>> Handle(GetTankCategoriesQuery request, CancellationToken cancellationToken)
    {
        var queryable = _mapper.ProjectTo<TankCategoryResponseModel>(_unitOfWork.TankCategoryRepository.GetAll().Where(x => x.DeletedAt == null).AsNoTracking());
        queryable = queryable.OrderBy(x => x.TankType).ThenBy(x => x.Id);
        var count = await queryable.CountAsync(cancellationToken);

        if (request.QueryFilter.PageNumber is not null && request.QueryFilter.PageSize is not null)
        {
            request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }
        
        var tankCategories = await queryable.ToListAsync(cancellationToken);
        
        return count == 0
            ? new PaginatedList<TankCategoryResponseModel>([], 0, 0, 0)
            : new PaginatedList<TankCategoryResponseModel>(tankCategories, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }
}