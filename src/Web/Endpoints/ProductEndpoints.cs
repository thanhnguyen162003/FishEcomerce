using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.Utils;
using Application.Products.Commands.CreateFishProduct;
using Application.Products.Commands.CreateTankProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateTankProduct;
using Application.Products.Queries.FishQueries;
using Application.Products.Queries.TankQueries;
using Carter;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/product");
        
        group.MapDelete("tank/{productId}", DeleteProduct).WithName(nameof(DeleteProduct));

        group.MapPost("tank", CreateTankProduct).WithName(nameof(CreateTankProduct));
        group.MapPatch("tank/{productId}", UpdateTankProduct).WithName(nameof(UpdateTankProduct));
        
        group.MapGet("tanks", GetAllTankProducts).WithName(nameof(GetAllTankProducts));
        group.MapGet("tank/{productId}", GetTankProductById).WithName(nameof(GetTankProductById));

        group.MapGet("fishsproduct", GetAllFishProducts).WithName(nameof(GetAllFishProducts));
        group.MapGet("fishs", GetAllFishs).WithName(nameof(GetAllFishs));
        group.MapPost("fish", CreateFishProduct).WithName(nameof(CreateFishProduct));
    }
    
    private async Task<IResult> GetAllFishs(ISender sender, [AsParameters] FishQueryFilter query, HttpContext httpContext)
    {
        var result = await sender.Send(new QueryFishCommand { QueryFilter = query });
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
    
    private async Task<IResult> GetAllFishProducts(ISender sender, [AsParameters] FishQueryFilter query, HttpContext httpContext)
    {
        var result = await sender.Send(new QueryFishProductCommand { QueryFilter = query });
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
    
    private async Task<IResult> CreateTankProduct(ISender sender,[FromBody, Required] TankProductCreateModel tankProduct, ValidationHelper<TankProductCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateTankProductCommand{TankProductCreateModel = tankProduct});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    public async Task<IResult> CreateFishProduct(ISender sender, [FromBody, Required] FishProductCreateModel fishProduct, ValidationHelper<FishProductCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(fishProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new CreateFishProductCommand { FishProductCreateModel = fishProduct });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> UpdateTankProduct(ISender sender,[FromBody, Required] TankProductUpdateModel tankProduct, [Required] Guid productId ,ValidationHelper<TankProductUpdateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateTankProductCommand{ProductId = productId, TankProductUpdateModel = tankProduct});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> DeleteProduct(ISender sender, Guid productId)
    {
        var result = await sender.Send(new DeleteProductCommand{ProductId = productId});

        if (result.Status == HttpStatusCode.NotFound)
        {
            return Results.NotFound(result);
        }
        
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> GetAllTankProducts(ISender sender, [AsParameters] TankQueryFilter query,
        HttpContext httpContext)
    {
        var result = await sender.Send(new GetTankWithPaginationQuery { QueryFilter = query });
        
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

    private async Task<IResult> GetTankProductById(ISender sender, Guid productId)
    {
        var result = await sender.Send(new GetTankByIdQuery{Id = productId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.NotFound(result);
    }
}