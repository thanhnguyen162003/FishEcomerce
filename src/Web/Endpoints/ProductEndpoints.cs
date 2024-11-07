using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.Utils;
using Application.Images.Commands;
using Application.Products.Commands.CreateFishProduct;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.CreateTankProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateFishProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Commands.UpdateTankProduct;
using Application.Products.Queries;
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
        group.MapGet("slug/{slug}", GetProductBySlug).WithName(nameof(GetProductBySlug));
        group.MapDelete("{productId}", DeleteProduct).WithName(nameof(DeleteProduct));

        group.MapPost("tank", CreateTankProduct).RequireAuthorization("Admin&Staff").WithName(nameof(CreateTankProduct));
        group.MapPatch("tank/{productId}", UpdateTankProduct).RequireAuthorization("Admin&Staff").WithName(nameof(UpdateTankProduct));
        group.MapGet("tanks", GetAllTankProducts).WithName(nameof(GetAllTankProducts));
        group.MapGet("tank/{productId}", GetTankProductById).WithName(nameof(GetTankProductById));

        group.MapGet("fishs", GetAllFishProducts).WithName(nameof(GetAllFishProducts));
        group.MapGet("fish/{id}", GetFish).WithName(nameof(GetFish));
        group.MapPatch("fish/{productId}", UpdateFishProduct).RequireAuthorization("Admin&Staff").WithName(nameof(UpdateFishProduct));
        group.MapPost("fish", CreateFishProduct).RequireAuthorization("Admin&Staff").WithName(nameof(CreateFishProduct));
        
        group.MapPost("", CreateProduct).RequireAuthorization().WithName(nameof(CreateProduct));
        group.MapPatch("{productId}", UpdateProduct).RequireAuthorization("Admin&Staff").WithName(nameof(UpdateProduct));
        
    }
   
    private async Task<IResult> DeleteProduct(ISender sender, Guid productId)
    {
        var result = await sender.Send(new DeleteProductCommand{ProductId = productId});
        
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> GetProductBySlug(ISender sender, string slug)
    {
        var result = await sender.Send(new GetProductBySlugQuery { Slug = slug });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.NotFound(result);
    }
    
    // Fish
    public async Task<IResult> CreateFishProduct(ISender sender, [FromForm, Required] FishProductCreateModel fishProduct,
        ValidationHelper<FishProductCreateModel> validationHelper, HttpRequest httpRequest)
    {
        fishProduct.ImageFiles = httpRequest.Form.Files;
        var fishJson = httpRequest.Form["fishModel"];
        var awardJson = httpRequest.Form["fishAward"];
        if (!string.IsNullOrWhiteSpace(fishJson) && !fishJson.ToString().Trim().Equals("{}"))
        {
            fishProduct.FishModel = JsonConvert.DeserializeObject<FishCreateRequestModel>(fishJson!);
        }

        if (!string.IsNullOrWhiteSpace(awardJson))
        {
            fishProduct.FishAward = JsonConvert.DeserializeObject<IEnumerable<FishAwardCreateRequestModel>>(awardJson!);
        }
        
        var (isValid, response) = await validationHelper.ValidateAsync(fishProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new CreateFishProductCommand { FishProductCreateModel = fishProduct });
        return result.Status == HttpStatusCode.Created ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> UpdateFishProduct(ISender sender, [FromForm] FishProductUpdateModel fishProduct, [Required] Guid productId,
        ValidationHelper<FishProductUpdateModel> validationHelper, HttpRequest httpRequest)
    {
        fishProduct.UpdateImages = httpRequest.Form.Files.Any() ? httpRequest.Form.Files : new List<IFormFile>();
        fishProduct.DeleteImages ??= new List<Guid>();
        var fishJson = httpRequest.Form["fishModel"];
        if (!string.IsNullOrWhiteSpace(fishJson) && !fishJson.ToString().Trim().Equals("{}"))
        {
            fishProduct.FishModel = JsonConvert.DeserializeObject<FishUpdateRequestModel>(fishJson);
        }

        var (isValid, response) = await validationHelper.ValidateAsync(fishProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new UpdateFishProductCommand { ProductId = productId, FishProductUpdateModel = fishProduct });

        if (result.Status != HttpStatusCode.OK)
        {
            return Results.BadRequest(result);
        }

        if (!fishProduct.DeleteImages.Any() && !fishProduct.UpdateImages.Any())
        {
            return Results.Ok(result);
        }
        
        var updateImages = await sender.Send(new UpdateImageCommand
        {
            ProductId = productId,
            DeleteImages = fishProduct.DeleteImages,
            UpdateImages = fishProduct.UpdateImages
        });

        return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
    }

    private async Task<IResult> GetFish(ISender sender, Guid id, HttpContext httpContext)
    {
        var result = await sender.Send(new GetFishProductByIdQuery { Id = id });
        return JsonHelper.Json(result);
    }

    private async Task<IResult> GetAllFishProducts(ISender sender, [AsParameters] FishQueryFilter query, HttpContext httpContext)
    {
        query.ApplyDefaults();
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
    
    // Tank
    private async Task<IResult> CreateTankProduct(ISender sender,[FromForm, Required] TankProductCreateModel tankProduct,
        ValidationHelper<TankProductCreateModel> validationHelper, HttpRequest httpRequest)
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
    
    private async Task<IResult> UpdateTankProduct(ISender sender,[FromForm] TankProductUpdateModel tankProduct, [Required] Guid productId ,
        ValidationHelper<TankProductUpdateModel> validationHelper, HttpRequest httpRequest)
    {
        tankProduct.UpdateImages = httpRequest.Form.Files.Any() ? httpRequest.Form.Files : new List<IFormFile>();
        tankProduct.DeleteImages ??= new List<Guid>();
        var tankJson = httpRequest.Form["tankModel"];
        if (!string.IsNullOrWhiteSpace(tankJson) && !tankJson.ToString().Trim().Equals("{}"))
        {
            tankProduct.TankModel = JsonConvert.DeserializeObject<TankUpdateModel>(tankJson);
        }
        
        var (isValid, response) = await validationHelper.ValidateAsync(tankProduct);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateTankProductCommand{ProductId = productId, TankProductUpdateModel = tankProduct});
        
        if (result.Status != HttpStatusCode.OK)
        {
            return Results.BadRequest(result);
        }

        if (!tankProduct.DeleteImages.Any() && !tankProduct.UpdateImages.Any())
        {
            return Results.Ok(result);
        }
        
        var updateImages = await sender.Send(new UpdateImageCommand
        {
            ProductId = productId,
            DeleteImages = tankProduct.DeleteImages,
            UpdateImages = tankProduct.UpdateImages
        });
                
        return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
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
    
    // Product
    private async Task<IResult> CreateProduct(ISender sender, ProductCreateModel model,
        ValidationHelper<ProductCreateModel> validationHelper, HttpRequest httpRequest)
    {
        model.ImageFiles = httpRequest.Form.Files;
        
        var (isValid, response) = await validationHelper.ValidateAsync(model);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateProductCommand(){ProductCreateModel = model});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> UpdateProduct(ISender sender, [FromForm] ProductUpdateModel model, [Required] Guid productId,
        ValidationHelper<ProductUpdateModel> validationHelper, HttpRequest httpRequest)
    {
        
        model.UpdateImages = httpRequest.Form.Files.Any() ? httpRequest.Form.Files : new List<IFormFile>();
        model.DeleteImages ??= new List<Guid>();
        var (isValid, response) = await validationHelper.ValidateAsync(model);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateProductCommand(){ProductId = productId, ProductUpdateModel = model});
        
        if (result.Status != HttpStatusCode.OK)
        {
            return Results.BadRequest(result);
        }
        
        if (!model.DeleteImages.Any() && !model.UpdateImages.Any())
        {
            return Results.Ok(result);
        }
        
        var updateImages = await sender.Send(new UpdateImageCommand
        {
            ProductId = productId,
            DeleteImages = model.DeleteImages,
            UpdateImages = model.UpdateImages
        });
        
        return updateImages.Status == HttpStatusCode.OK ? Results.Ok(updateImages) : Results.BadRequest(updateImages);
    }
}