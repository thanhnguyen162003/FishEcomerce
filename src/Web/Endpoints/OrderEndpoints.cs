using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.Utils;
using Application.Order.Command;
using Application.Order.Command.CancelOrder;
using Application.Order.Command.CreateOrder;
using Application.Order.Command.UpdateOrder;
using Application.Order.Queries;
using Carter;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/order");
        group.MapPost("", CreateOrder).WithName(nameof(CreateOrder));
        group.MapGet("", GetOrdersWithPagination).WithName(nameof(GetOrdersWithPagination));
        group.MapPatch("{orderId}", UpdateOrder).WithName(nameof(UpdateOrder));
        group.MapPatch("cancel", CancelOrder).WithName(nameof(CancelOrder));
    }

    private async Task<IResult> CreateOrder(ISender sender, [FromBody, Required] OrderCreateModel model,
        CancellationToken cancellationToken, ValidationHelper<OrderCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(model);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }

        var result = await sender.Send(new CreateOrderCommand() { OrderCreateModel = model }, cancellationToken);
        return result.Status == HttpStatusCode.BadRequest ? Results.BadRequest(result) : Results.Ok(result);
    }

    private async Task<IResult> GetOrdersWithPagination(ISender sender, [AsParameters] OrderQueryFilter query,
        CancellationToken cancellationToken, HttpContext httpContext)
    {
        query.ApplyDefaults();
        var result = await sender.Send(new GetOrdersByCustomerIdQuery() { QueryFilter = query }, cancellationToken);

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

    private async Task<IResult> UpdateOrder(ISender sender, [FromBody, Required] OrderUpdateModel model, Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateOrderCommand(){OrderId = orderId, OrderUpdateModel = model}, cancellationToken);
        return result.Status == HttpStatusCode.BadRequest ? Results.BadRequest(result) : Results.Ok(result);
    }
    
    private async Task<IResult> CancelOrder(ISender sender, Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelOrderCommand(){OrderId = orderId}, cancellationToken);
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    } 
}