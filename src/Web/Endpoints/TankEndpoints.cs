using System.Net;
using System.Windows.Input;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Products.Commands.CreateProduct;
using Application.Tanks.Commands.CreateTank;
using Carter;

namespace Web.Endpoints;

public class TankEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("api/tank")
            .MapPost("/", CreateTank).WithName(nameof(CreateTank));

    }

    private async Task<IResult> CreateTank(ISender sender, ProductCreateModel model)
    {
        var product = await sender.Send(new CreateProductCommand{ProductModel = model});

        if (product.Status == HttpStatusCode.BadRequest)
        {
            return Results.BadRequest(product);
        }
        
        var result = await sender.Send(new CreateTankCommand{ProductId = (Guid)product.Data, TankModel = model.TankModel});
        
        return result.Status == HttpStatusCode.Created ? Results.Ok(result) : Results.BadRequest(result);
    }
    
}