using Application.Common.Models;
using Application.Common.Models.FeedbackModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.Feedbacks.Queries;

public record GetFeedbackWithPaginationQuery() : IRequest<PaginatedList<FeedbackResponseModel>>
{
    public FeedBackQueryFilter QueryFilter { get; init; }
}

public class GetFeedbackWithPaginationQueryHandler : IRequestHandler<GetFeedbackWithPaginationQuery, PaginatedList<FeedbackResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;

    public GetFeedbackWithPaginationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PaginatedList<FeedbackResponseModel>> Handle(GetFeedbackWithPaginationQuery request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
        
        
        var queryable = _unitOfWork.FeedbackRepository.GetAll();
        if (request.QueryFilter.Rate != null)
        {
            queryable = queryable.Where(x => x.Rate == request.QueryFilter.Rate);
        }
        
        queryable = queryable.Skip((request.QueryFilter.PageNumber - 1) * request.QueryFilter.PageSize).Take(request.QueryFilter.PageSize);
        
        var feedbackList = await queryable.ToListAsync(cancellationToken);
        var feedbacks = _mapper.Map<List<FeedbackResponseModel>>(feedbackList);
        var count = feedbacks.Count;
        return count == 0
            ? new PaginatedList<FeedbackResponseModel>([], 0, 0, 0)
            : new PaginatedList<FeedbackResponseModel>(feedbacks, count, request.QueryFilter.PageNumber,
                request.QueryFilter.PageSize);
    }
}
