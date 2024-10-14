using Carter;

namespace Web.Endpoints;

public class PaymentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/payment");
        // group.MapPost("", Webhook).WithName(nameof(Webhook));
    }

    // private async Task<IResult> Webhook(ISender sender, )
}