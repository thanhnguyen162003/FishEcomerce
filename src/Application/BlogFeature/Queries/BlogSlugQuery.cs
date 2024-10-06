using Application.Common.Models;
using Application.Common.Models.BlogModel;
using Application.Common.UoW;
using AutoMapper;

namespace Application.BlogFeature.Queries;

public class BlogSlugQuery : IRequest<BlogResponseModel>
{
    public required string Slug { get; set; }
}

public class BlogSlugQueryHandler : IRequestHandler<BlogSlugQuery, BlogResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BlogSlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BlogResponseModel> Handle(BlogSlugQuery request, CancellationToken cancellationToken)
    {
        var blog = await _unitOfWork.BlogRepository.GetBlogBySlug(request.Slug, cancellationToken);
        var blogResponse = _mapper.Map<BlogResponseModel>(blog);
        return blogResponse;
    }
}

