using System.Net;
using Application.Common.Models.PaymentModels;
using Application.Payment.Commands;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Web.Endpoints;

public class PaymentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/payment");
        group.MapPost("webhook", Webhook).WithName(nameof(Webhook));
        group.MapPost("", CreatePayment).WithName(nameof(CreatePayment));
    }

    private async Task<IResult> Webhook(ISender sender, [FromBody] WebhookType type)
    {
        var result = await sender.Send(new WebhookCommand(){Type = type});
        return Results.Ok(result);
    }

    private async Task<IResult> CreatePayment(ISender sender,[FromBody] long orderCode)
    {
        var result = await sender.Send(new CreatePaymentCommand() { OrderCode = orderCode });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}