using System.Net;
using Application.Common.Models;
using Application.Common.Models.BlogModel;
using Application.Common.UoW;

namespace Application.BlogFeature.Commands;

public class UpdateBlogCommand : IRequest<ResponseModel>
{
    public Guid Id { get; set; }
    public BlogUpdateRequestModel BlogUpdateRequestModel { get; set; }
}

public class UpdateBlogCommandHandler : IRequestHandler<UpdateBlogCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBlogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _unitOfWork.BlogRepository.GetBlogById(request.Id, cancellationToken);
        if (blog == null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Blog not found", null);
        }
        _mapper.Map(request.BlogUpdateRequestModel, blog);
        blog.UpdatedAt = DateTime.Now;
        var result = await _unitOfWork.BlogRepository.UpdateBlog(blog, cancellationToken);
        if (!result)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Blog update failed", null);
        }
        return new ResponseModel(HttpStatusCode.OK, "Blog updated successfully", null);
    }
}

