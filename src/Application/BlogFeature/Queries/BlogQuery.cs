using Application.Common.Models.BlogModel;
using Application.Common.UoW;
using Domain.CustomEntities;
using Domain.QueriesFilter;
using Microsoft.Extensions.Options;

namespace Application.BlogFeature.Queries;

public record BlogQuery : IRequest<PagedList<BlogResponseModel>>
{
    public BlogQueryFilter BlogQueryFilter { get; set; }
}

public class BlogQueryHandler : IRequestHandler<BlogQuery, PagedList<BlogResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;

    public BlogQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PagedList<BlogResponseModel>> Handle(BlogQuery request, CancellationToken cancellationToken)
    {
        request.BlogQueryFilter.PageNumber = request.BlogQueryFilter.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : request.BlogQueryFilter.PageNumber;
        request.BlogQueryFilter.PageSize = request.BlogQueryFilter.PageSize == 0 ? _paginationOptions.DefaultPageSize : request.BlogQueryFilter.PageSize;
        var listBlog = await _unitOfWork.BlogRepository.GetAllAsync(request.BlogQueryFilter, cancellationToken);
        if (!listBlog.Any())
        {
            return new PagedList<BlogResponseModel>(new List<BlogResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<BlogResponseModel>>(listBlog);
        return PagedList<BlogResponseModel>.Create(mapperList, request.BlogQueryFilter.PageNumber, request.BlogQueryFilter.PageSize);
    }
}   