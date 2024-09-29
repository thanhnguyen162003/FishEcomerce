using AutoMapper;
using Carter;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class TankEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1");
        // group.MapGet("demoendpoint",GetChapters).WithName(nameof(GetChapters));
    }

    // public static async Task<IResult> GetChapters([AsParameters] ChapterQueryFilter queryFilter, ISender sender,
    //     IMapper mapper, CancellationToken cancellationToken, HttpContext httpContext)
    // {
    //     var query = new ChapterQuery()
    //     {
    //         QueryFilter = queryFilter
    //     };
    //     var result = await sender.Send(query, cancellationToken);
    //     var metadata = new Metadata
    //     {
    //         TotalCount = result.TotalCount,
    //         PageSize = result.PageSize,
    //         CurrentPage = result.CurrentPage,
    //         TotalPages = result.TotalPages
    //     };
    //     httpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
    //     return JsonHelper.Json(result);
    // }
}