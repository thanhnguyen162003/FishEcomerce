using System.Net;
using System.Net.Http;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.ProductModels;
using Application.Common.Utils;
using Application.Products.Commands.BreedModels.CreateBreed;
using Application.Products.Commands.BreedModels.UpdateBreed;
using Application.Products.Queries.QueryBreed;
using Carter;
using Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class BreedEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/breed");
        group.MapPost("breed", CreateBreed).WithName(nameof(CreateBreed));
        group.MapPut("breed/{breedId}", UpdateBreed).WithName(nameof(UpdateBreed));
        group.MapGet("breed/{breedId}", GetBreed).WithName(nameof(GetBreed));
    }

    public static async Task<IResult> CreateBreed(ISender sender, [FromBody] BreedCreateRequestModel breed, ValidationHelper<BreedCreateRequestModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(breed);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        var result = await sender.Send(new CreateBreedCommand{CreateBreedModel = breed});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result.Data) : Results.BadRequest(result);
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

        
        return JsonHelper.Json(result);
    }
}