using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models.ProductModels;
using Application.Common.Utils;
using Application.Products.Commands.CreateFishProduct;
using Application.Products.Commands.CreateTankProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateTankProduct;
using Carter;
using Domain.Entites;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/product");
        group.MapPost("tank", CreateTankProduct).WithName(nameof(CreateTankProduct));
        group.MapPatch("tank/{productId}", UpdateTankProduct).WithName(nameof(UpdateTankProduct));
        group.MapDelete("tank/{productId}", DeleteProduct).WithName(nameof(DeleteProduct));

        group.MapPost("fish", CreateFishProduct).WithName(nameof(CreateFishProduct));
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

        if (result.Status == HttpStatusCode.OK)
        {
            return Results.Ok(result);
        }
        
        return result.Status == HttpStatusCode.BadRequest ? Results.BadRequest(result) : Results.NotFound(result);
    }
}