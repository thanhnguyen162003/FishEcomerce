using System.Net;
using Application.Common.Models;
using Application.Common.Models.BlogModel;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

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
    private readonly ICloudinaryService _cloudinaryService;

    public CreateBlogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<ResponseModel> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimsService.GetCurrentUserId;
        var blog = _mapper.Map<Domain.Entites.Blog>(request.BlogCreateRequestModel);
        blog.CreatedAt = DateTime.Now;
        blog.UpdatedAt = DateTime.Now;
        blog.DeletedAt = null;
        blog.Id = new UuidV7().Value;
        blog.StaffId = userId;
        blog.Slug = SlugHelper.GenerateSlug(blog.Title!);
        
        var upload = await _cloudinaryService.UploadAsync(request.BlogCreateRequestModel.Thumbnail);
        if (upload.Error is not null)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, upload.Error.Message);
        }
        
        var image = new Image()
        {
            Id = new UuidV7().Value,
            BlogId = blog.Id,
            PublicId = upload.PublicId,
            Link = upload.Url.ToString()
        };
        
        blog.Images.Add(image);
        
        var result = await _unitOfWork.BlogRepository.CreateBlog(blog, cancellationToken);
        return result ? new ResponseModel(HttpStatusCode.OK, "Blog created successfully", blog.Id) : new ResponseModel(HttpStatusCode.BadRequest, "Blog creation failed", null);
    }
}