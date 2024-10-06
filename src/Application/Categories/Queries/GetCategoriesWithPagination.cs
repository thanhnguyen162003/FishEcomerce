using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.Categories.Queries;

public record GetCategoriesWithPagination : IRequest<PaginatedList<CategoryResponseModel>>
{
    public CategoryQueryFilter QueryFilter { get; init; }
}

public class GetCategoriesWithPaginationHandler : IRequestHandler<GetCategoriesWithPagination, PaginatedList<CategoryResponseModel>>
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

    public async Task<PaginatedList<CategoryResponseModel>> Handle(GetCategoriesWithPagination request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;

        var queryable = _unitOfWork.CategoryRepository.GetAll().AsNoTracking();

        queryable = queryable.Skip((request.QueryFilter.PageNumber - 1) * request.QueryFilter.PageSize).Take(request.QueryFilter.PageSize);
        queryable = queryable.OrderByDescending(x => x.Level);
        var categoryList = await queryable.ToListAsync(cancellationToken);
        var categories = _mapper.Map<List<CategoryResponseModel>>(categoryList);
        var count = categories.Count;
        return count == 0
            ? new PaginatedList<CategoryResponseModel>([], 0, 0, 0)
            : new PaginatedList<CategoryResponseModel>(categories, count, request.QueryFilter.PageNumber,
                request.QueryFilter.PageSize);
    }
}