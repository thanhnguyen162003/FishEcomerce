using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.Categories.Queries;

public record GetCategoriesQuery : IRequest<PaginatedList<CategoryResponseModel>>
{
    public CategoryQueryFilter QueryFilter { get; init; }
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryResponseModel>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PaginationOptions _paginationOptions;
    
    public GetCategoriesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _paginationOptions = paginationOptions.Value;
    }
    
    public async Task<PaginatedList<CategoryResponseModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.CategoryRepository.GetAll()
            .Where(x => x.DeletedAt == null)
            .AsNoTracking();

        if (request.QueryFilter.CategoryType != null)
        {
            queryable = queryable.Where(x => x.Type == request.QueryFilter.CategoryType.ToString().ToLower());
        }
        
        queryable = queryable.OrderBy(x => x.Name).ThenBy(x => x.Id);
        var count = await queryable.CountAsync(cancellationToken);

        if (request.QueryFilter.PageNumber is not null && request.QueryFilter.PageSize is not null)
        {
            request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }
        
        var categories = await queryable.ToListAsync(cancellationToken);
        
        var mapper = _mapper.Map<List<CategoryResponseModel>>(categories);
        
        return count == 0
            ? new PaginatedList<CategoryResponseModel>([], 0, 0, 0)
            : new PaginatedList<CategoryResponseModel>(mapper, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }
}