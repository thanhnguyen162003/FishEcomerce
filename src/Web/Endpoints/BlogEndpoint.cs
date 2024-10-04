using Application.BlogFeature.Commands;
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
}