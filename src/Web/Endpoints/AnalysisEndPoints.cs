using System.ComponentModel.DataAnnotations;
using Application.AnalysisFeature.Queries.GetRegisterQuery;
using Application.AnalysisFeature.Queries.GetSalesQuery;
using Carter;

namespace Web.Endpoints;

public class AnalysisEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/analysis");
        group.MapGet("customers/day", GetRegisterByDay).WithName(nameof(GetRegisterByDay));
        group.MapGet("customers/month", GetRegisterByMonth).WithName(nameof(GetRegisterByMonth));
        group.MapGet("customers/year", GetRegisterByYear).WithName(nameof(GetRegisterByYear));
        group.MapGet("sales/day", GetSalesByDay).WithName(nameof(GetSalesByDay));
        group.MapGet("sales/month", GetSalesByMonth).WithName(nameof(GetSalesByMonth));
        group.MapGet("sales/year", GetSalesByYear).WithName(nameof(GetSalesByYear));

    }

    private async Task<IResult> GetRegisterByDay(ISender sender,[Required] int year,[Required] int month,[Required] int day)
    {
        var result = await sender.Send(new GetRegisterByDayQuery{Day = day, Month = month, Year = year});
        return Results.Ok(result);
    }
    
    private async Task<IResult> GetRegisterByMonth(ISender sender,[Required] int year,[Required] int month)
    {
        var result = await sender.Send(new GetRegisterByMonthQuery{Month = month, Year = year});
        return Results.Ok(result);
    }
    
    private async Task<IResult> GetRegisterByYear(ISender sender,[Required] int year)
    {
        var result = await sender.Send(new GetRegisterByYearQuery{Year = year});
        return Results.Ok(result);
    }
    
    private async Task<IResult> GetSalesByDay(ISender sender,[Required] int year,[Required] int month,[Required] int day)
    {
        var result = await sender.Send(new GetSalesByDayQuery(){Day = day, Month = month, Year = year});
        return Results.Ok(result);
    }
    
    private async Task<IResult> GetSalesByMonth(ISender sender,[Required] int year,[Required] int month)
    {
        var result = await sender.Send(new GetSalesByMonthQuery(){Month = month, Year = year});
        return Results.Ok(result);
    }
    
    private async Task<IResult> GetSalesByYear(ISender sender,[Required] int year)
    {
        var result = await sender.Send(new GetSalesByYearQuery{Year = year});
        return Results.Ok(result);
    }
}