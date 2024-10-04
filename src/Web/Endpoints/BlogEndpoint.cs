using Application.BlogFeature.Commands;
using Application.BlogFeature.Queries;
using Application.Common.Models.BlogModel;
using Application.Common.Utils;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class BlogEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/blog");
        group.MapPost("", CreateBlog).WithName(nameof(CreateBlog)).RequireAuthorization("Supplier");
        group.MapGet("slug/{slug}", GetBlogBySlug).WithName(nameof(GetBlogBySlug));
        group.MapGet("{id}", GetBlogById).WithName(nameof(GetBlogById));
        group.MapPut("{id}", UpdateBlog).WithName(nameof(UpdateBlog)).RequireAuthorization("Supplier");
    }

    public async Task<IResult> CreateBlog(ISender sender, [FromBody] BlogCreateRequestModel blog, ValidationHelper<BlogCreateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(blog);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateBlogCommand{BlogCreateRequestModel = blog});
        return JsonHelper.Json(result);
    }
    public async Task<IResult> UpdateBlog(ISender sender, [FromBody] BlogUpdateRequestModel blog, ValidationHelper<BlogUpdateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(blog);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new UpdateBlogCommand{BlogUpdateRequestModel = blog});
        return JsonHelper.Json(result);
    }
    public async Task<IResult> GetBlogBySlug(ISender sender, string slug)
    {
        var result = await sender.Send(new BlogSlugQuery{Slug = slug});
        return JsonHelper.Json(result);
    }
    public async Task<IResult> GetBlogById(ISender sender, Guid id)
    {
        var result = await sender.Send(new BlogDetailIdQuery{Id = id});
        return JsonHelper.Json(result);
    }
}
