using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models.OrderModels;
using Application.Order.Command;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/order");
        group.MapPost("", CreateOrder).WithName(nameof(CreateOrder));
    }

    private async Task<IResult> CreateOrder(ISender sender, [FromBody, Required] OrderCreateModel model,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateOrderCommand() { OrderCreateModel = model });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}