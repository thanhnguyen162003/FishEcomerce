using System.Net;
using Application.Common.Models;
using Application.Common.Models.BlogModel;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.BlogFeature.Commands;

public record CreateBlogCommand : IRequest<ResponseModel>
{
    public BlogCreateRequestModel BlogCreateRequestModel { get; set; }
}

public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public CreateBlogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimsService.GetCurrentUserId;
        Domain.Entites.Blog blog = _mapper.Map<Domain.Entites.Blog>(request.BlogCreateRequestModel);
        blog.CreatedAt = DateTime.Now;
        blog.UpdatedAt = DateTime.Now;
        blog.DeletedAt = null;
        blog.Id = new UuidV7().Value;
        blog.StaffId = userId;
        blog.Slug = SlugHelper.GenerateSlug(blog.Title!);
        var result = await _unitOfWork.BlogRepository.CreateBlog(blog, cancellationToken);
        if (result)
        {
            return new ResponseModel(HttpStatusCode.OK, "Blog created successfully", blog.Id);
        }
        return new ResponseModel(HttpStatusCode.BadRequest, "Blog creation failed", null);
    }
}