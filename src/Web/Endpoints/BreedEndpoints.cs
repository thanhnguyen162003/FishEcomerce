using System.Net;
using Application.Breeds.Commands.CreateBreed;
using Application.Breeds.Commands.UpdateBreed;
using Application.Breeds.Queries;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Utils;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class BreedEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/breed");
        group.MapPost("", CreateBreed).WithName(nameof(CreateBreed));
        group.MapPut("{breedId}", UpdateBreed).WithName(nameof(UpdateBreed));
        group.MapGet("", GetBreed).WithName(nameof(GetBreed));
    }

    public static async Task<IResult> CreateBreed(ISender sender, [FromBody] BreedCreateRequestModel breed, ValidationHelper<BreedCreateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(breed);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateBreedCommand{CreateBreedModel = breed});
        return result.Status == HttpStatusCode.Created ? Results.Ok(result.Data) : Results.BadRequest(result);
    }

    public static async Task<IResult> UpdateBreed(ISender sender, [FromBody] BreedUpdateRequestModel breed, Guid id, ValidationHelper<BreedUpdateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(breed);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new UpdateBreedCommand{UpdateBreedModel = breed, Id = id});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result.Data) : Results.BadRequest(result);
    }
    
    public static async Task<IResult> GetBreed([AsParameters] BreedQueryFilter queryFilter, ISender sender, HttpContext httpContext)
    {
        var result = await sender.Send(new QueryBreedCommand {QueryFilter = queryFilter});
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