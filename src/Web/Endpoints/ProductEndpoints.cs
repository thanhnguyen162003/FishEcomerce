using System.Net;
using Application.Common.Models.ProductModels;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Carter;
using Domain.Entites;

namespace Web.Endpoints;

public class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/product");
        group.MapPost("tank", CreateTankProduct).WithName(nameof(CreateTankProduct));
        group.MapDelete("tank/{productId}", DeleteProduct).WithName(nameof(DeleteProduct));
    }

    private async Task<IResult> CreateTankProduct(ISender sender, CreateTankProductModel tankProduct)
    {
        var result = await sender.Send(new CreateProductCommand{CreateTankProductModel = tankProduct});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result.Data) : Results.BadRequest(result);
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