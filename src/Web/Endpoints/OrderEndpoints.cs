using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.Utils;
using Application.Orders.Command.CancelOrder;
using Application.Orders.Command.CreateOrder;
using Application.Orders.Command.UpdateOrder;
using Application.Orders.Queries;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/order");
        group.MapPost("", CreateOrder).WithName(nameof(CreateOrder)).RequireAuthorization("Customer");
        group.MapGet("", GetCustomerOrdersWithPagination).WithName(nameof(GetCustomerOrdersWithPagination)).RequireAuthorization("Customer");
        group.MapGet("{orderCode}", GetCustomerOrderByOrderCode).WithName(nameof(GetCustomerOrderByOrderCode)).RequireAuthorization("Customer");
        group.MapPatch("{orderId}", UpdateOrder).WithName(nameof(UpdateOrder)).RequireAuthorization("Admin&Staff");
        group.MapPatch("cancel", CancelOrder).WithName(nameof(CancelOrder)).RequireAuthorization("Customer");
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

    private async Task<IResult> GetCustomerOrdersWithPagination(ISender sender, [AsParameters] OrderQueryFilter query,
        CancellationToken cancellationToken, HttpContext httpContext)
    {
        query.ApplyDefaults();
        var result = await sender.Send(new GetCustomerOrdersByCustomerIdQuery() { QueryFilter = query }, cancellationToken);

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
    
    private async Task<IResult> GetCustomerOrderByOrderCode(ISender sender, long orderCode, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCustomerOrderByOrderCodeQuery(){OrderCode = orderCode}, cancellationToken);
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> UpdateOrder(ISender sender, [FromBody, Required] OrderUpdateModel model, Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateOrderCommand(){OrderId = orderId, OrderUpdateModel = model}, cancellationToken);
        return result.Status == HttpStatusCode.BadRequest ? Results.BadRequest(result) : Results.Ok(result);
    }
    
    private async Task<IResult> CancelOrder(ISender sender, Guid orderId, bool broke,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelOrderCommand(){OrderId = orderId}, cancellationToken);
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    } 
}