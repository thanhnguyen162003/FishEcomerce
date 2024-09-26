using System.Net;
using Carter;
using FishEcomerce.Application.Common.Models.ProductModels;
using FishEcomerce.Domain.Entities;
using Microsoft.Extensions.DependencyInjection.Products.Commands;

namespace Microsoft.Extensions.DependencyInjection.Endpoints;

public class Products : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("api/proudct")
            .MapPost("/", CreateProduct);
    }

    private async Task<IResult> CreateProduct(ISender sender, ProductRequestModel model)
    {
        var result = await sender.Send(new CreateProductCommand{Model = model});

        return result.Status == HttpStatusCode.Created ? Results.Ok(result) : Results.BadRequest(result);
    }
}
