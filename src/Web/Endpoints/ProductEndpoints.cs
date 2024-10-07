using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.Utils;
using Application.Images.Commands;
using Application.Products.Commands.CreateFishProduct;
using Application.Products.Commands.CreateTankProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateFishProduct;
using Application.Products.Commands.UpdateTankProduct;
using Application.Products.Queries.FishQueries;
using Application.Products.Queries.TankQueries;
using Carter;
using Domain.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/product").DisableAntiforgery();
        
        group.MapDelete("tank/{productId}", DeleteProduct).WithName(nameof(DeleteProduct));

        group.MapPost("tank", CreateTankProduct).WithName(nameof(CreateTankProduct));
        group.MapPatch("tank/{productId}", UpdateTankProduct).WithName(nameof(UpdateTankProduct));
        
        group.MapGet("tanks", GetAllTankProducts).WithName(nameof(GetAllTankProducts));
        group.MapGet("tank/{productId}", GetTankProductById).WithName(nameof(GetTankProductById));

        group.MapGet("fishsproduct", GetAllFishProducts).WithName(nameof(GetAllFishProducts));
        group.MapGet("fishs", GetAllFishs).WithName(nameof(GetAllFishs));
        group.MapGet("fish/{id}", GetFish).WithName(nameof(GetFish));
        group.MapPatch("fish/{productId}", UpdateFishProduct).RequireAuthorization().WithName(nameof(UpdateFishProduct));
        group.MapPost("fish", CreateFishProduct).RequireAuthorization().WithName(nameof(CreateFishProduct));
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

    private async Task<IResult> GetFish(ISender sender, Guid id, HttpContext httpContext)
    {
        var result = await sender.Send(new GetFishProductByIdQuery { Id = id });
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
    
    private async Task<IResult> CreateTankProduct(ISender sender,[FromForm, Required] TankProductCreateModel tankProduct, ValidationHelper<TankProductCreateModel> validationHelper, HttpRequest httpRequest)
    {
        tankProduct.ImageFiles = httpRequest.Form.Files;
        var tankJson = httpRequest.Form["tankModel"];
        if (!string.IsNullOrWhiteSpace(tankJson))
        {
            tankProduct.TankModel = JsonConvert.DeserializeObject<TankCreateModel>(tankJson!);
        }
        var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateTankProductCommand{TankProductCreateModel = tankProduct});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    public async Task<IResult> CreateFishProduct(ISender sender, [FromForm, Required] FishProductCreateModel fishProduct, ValidationHelper<FishProductCreateModel> validationHelper, HttpRequest httpRequest)
    {
        fishProduct.ImageFiles = httpRequest.Form.Files;
        var fishJson = httpRequest.Form["fishModel"];
        if (!string.IsNullOrWhiteSpace(fishJson))
        {
            fishProduct.FishModel = JsonConvert.DeserializeObject<FishCreateRequestModel>(fishJson!);
        }
        var (isValid, response) = await validationHelper.ValidateAsync(fishProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new CreateFishProductCommand { FishProductCreateModel = fishProduct });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    private async Task<IResult> UpdateFishProduct(ISender sender, [FromForm, Required] FishProductUpdateModel    fishProduct, [Required] Guid productId, ValidationHelper<FishProductUpdateModel> validationHelper, HttpRequest httpRequest)
    {
        fishProduct.UpdateImages = httpRequest.Form.Files;
        var fishJson = httpRequest.Form["tankModel"];
        if (!string.IsNullOrWhiteSpace(fishJson) || !fishJson.ToString().Trim().Equals("{}"))
        {
            fishProduct.FishModel = JsonConvert.DeserializeObject<FishUpdateRequestModel>(fishJson);
        }

        var (isValid, response) = await validationHelper.ValidateAsync(fishProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new UpdateFishProductCommand { ProductId = productId, FishProductUpdateModel = fishProduct });

        if (result.Status == HttpStatusCode.OK)
        {
            if (fishProduct.DeleteImages.Any() || fishProduct.UpdateImages.Any())
            {
                var updateImages = await sender.Send(new UpdateImageCommand
                {
                    ProductId = productId,
                    DeleteImages = fishProduct.DeleteImages,
                    UpdateImages = fishProduct.UpdateImages
                });

                return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
            }
        }
        return Results.BadRequest(result);
    }
    private async Task<IResult> UpdateTankProduct(ISender sender,[FromForm, Required] TankProductUpdateModel tankProduct, [Required] Guid productId ,ValidationHelper<TankProductUpdateModel> validationHelper, HttpRequest httpRequest)
    {
        
        tankProduct.UpdateImages = httpRequest.Form.Files;
        var tankJson = httpRequest.Form["tankModel"];
        if (!string.IsNullOrWhiteSpace(tankJson) || !tankJson.ToString().Trim().Equals("{}"))
        {
            tankProduct.TankModel = JsonConvert.DeserializeObject<TankUpdateModel>(tankJson);
        }
        
        var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateTankProductCommand{ProductId = productId, TankProductUpdateModel = tankProduct});
        
        if (result.Status == HttpStatusCode.OK)
        {
            if (tankProduct.DeleteImages.Any() || tankProduct.UpdateImages.Any())
            {
                var updateImages = await sender.Send(new UpdateImageCommand
                {
                    ProductId = productId, DeleteImages = tankProduct.DeleteImages,
                    UpdateImages = tankProduct.UpdateImages
                });
                
                return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
            }
        }
        return Results.BadRequest(result);
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
        query.ApplyDefaults();
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