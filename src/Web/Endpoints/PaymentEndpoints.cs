using System.Net;
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
        group.MapPost("", Webhook).WithName(nameof(Webhook));
    }

    private async Task<IResult> Webhook(ISender sender, [FromBody] WebhookType type)
    {
        var result = await sender.Send(new WebhookCommand(){Type = type});
        return Results.Ok(result);
    }
}