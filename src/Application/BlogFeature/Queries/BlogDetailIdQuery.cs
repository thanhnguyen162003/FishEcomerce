using Application.Common.Models.BlogModel;
using Application.Common.UoW;

namespace Application.BlogFeature.Queries;

public class BlogDetailIdQuery : IRequest<BlogResponseModel>
{
    public Guid Id { get; set; }
}

public class BlogDetailIdQueryHandler : IRequestHandler<BlogDetailIdQuery, BlogResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BlogDetailIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BlogResponseModel> Handle(BlogDetailIdQuery request, CancellationToken cancellationToken)
    {
        var blog = await _unitOfWork.BlogRepository.GetBlogById(request.Id, cancellationToken);
        var blogResponse = _mapper.Map<BlogResponseModel>(blog);
        return blogResponse;
    }
}


