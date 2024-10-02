using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishAwardModels;
using Application.Common.Utils;
using Application.FishAwards.Commands.CreateFishAward;
using Application.FishAwards.Commands.UpdateFishAward;
using Application.FishAwards.Queries;
using Carter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class FishAwardEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/");
        group.MapPost("fishAward/{fishId}", CreateFishAward).WithName(nameof(CreateFishAward));
        group.MapPut("fishAward/{id}", UpdateFishAward).WithName(nameof(UpdateFishAward));
        group.MapGet("fishAward", GetFishAward).WithName(nameof(GetFishAward));
    }

    public static async Task<IResult> CreateFishAward(ISender sender, Guid fishId, [FromBody] FishAwardCreateRequestModel fishaward, ValidationHelper<FishAwardCreateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(fishaward);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateFishAwardCommand{CreateFishAwardModel = fishaward, Id = fishId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result.Data) : Results.BadRequest(result);
    }

    public static async Task<IResult> UpdateFishAward(ISender sender, [FromBody] FishAwardUpdateRequestModel fishAward, Guid id, ValidationHelper<FishAwardUpdateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(fishAward);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new UpdateFishAwardCommand{UpdateFishAwardModel = fishAward, Id = id});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result.Data) : Results.BadRequest(result);
    }
    
    public static async Task<IResult> GetFishAward([AsParameters] FishAwardQueryFilter queryFilter, ISender sender, HttpContext httpContext)
    {
        var result = await sender.Send(new QueryFishAwardCommand {QueryFilter = queryFilter});
        var metadata = new Metadata
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            CurrentPage = result.CurrentPage,
            TotalPages = result.TotalPages
        };
        httpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return JsonHelper.Json(result);
    }
}