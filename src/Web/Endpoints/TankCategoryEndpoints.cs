using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Categories.Queries;
using Application.Common.Models;
using Application.Common.Models.TankCategoryModels;
using Application.Common.Utils;
using Application.TankCategories.Commands.CreateTankCategory;
using Application.TankCategories.Commands.DeleteTankCategory;
using Application.TankCategories.Commands.UpdateTankCategory;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints;

public class TankCategoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/tankcategory");
        group.MapPost("", CreateTankCategory).WithName(nameof(CreateTankCategory));
        group.MapPatch("{tankcategoryId}", UpdateTankCategory).WithName(nameof(UpdateTankCategory));
        group.MapDelete("{tankcategoryId}", DeleteTankCategory).WithName(nameof(DeleteTankCategory));
        group.MapGet("", GetTankCategories);
    }

    private async Task<IResult> CreateTankCategory(ISender sender, [FromBody, Required] TankCategoryCreateModel tankCategoryModel, ValidationHelper<TankCategoryCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(tankCategoryModel);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateTankCategoryCommand{TankCategoryCreateModel = tankCategoryModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> UpdateTankCategory(ISender sender, [FromBody, Required] TankCategoryUpdateModel tankCategoryModel,[Required] Guid tankcategoryId, ValidationHelper<TankCategoryUpdateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(tankCategoryModel);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateTankCategoryCommand{TankCategoryId = tankcategoryId, TankCategoryUpdateModel = tankCategoryModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> DeleteTankCategory(ISender sender, [Required] Guid tankcategoryId)
    {
        var result = await sender.Send(new DeleteTankCategoryCommand{TankCategoryId = tankcategoryId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> GetTankCategories(ISender sender, [AsParameters] TankCategoryQueryFilter filter, HttpContext httpContext)
    {
        var result = await sender.Send(new GetTankCategoriesQuery(){QueryFilter = filter});
        
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
}