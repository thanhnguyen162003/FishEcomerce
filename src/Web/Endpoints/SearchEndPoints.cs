using Application.GlobalSearch.Queries;
using Carter;

namespace Web.Endpoints;

public class SearchEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/search");
        group.MapGet("", Search).WithName(nameof(Search));
    }

    private async Task<IResult> Search(ISender sender, string searchTerm)
    {
        var result = await sender.Send(new GetAllQuery(){Query = searchTerm});
        return Results.Ok(result);
    }
}