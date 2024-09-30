using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Categories.Commands.UpdateCategory;
using Application.Common.Models.CategoryModels;
using Application.Common.Utils;
using Application.TankCategories.Commands.CreateCategory;
using Application.TankCategories.Commands.DeleteCategory;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class CategoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/category");
        group.MapPost("", CreateCategory).WithName(nameof(CreateCategory));
        group.MapPatch("{categoryId}", UpdateCategory).WithName(nameof(UpdateCategory));
        group.MapDelete("{categoryId}", DeleteCategory).WithName(nameof(DeleteCategory));
    }

    private async Task<IResult> CreateCategory(ISender sender, [FromBody, Required] CategoryCreateModel categoryModel, ValidationHelper<CategoryCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(categoryModel);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateCategoryCommand{CategoryCreateModel = categoryModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> UpdateCategory(ISender sender, [FromBody, Required] CategoryUpdateModel categoryModel,[Required] Guid categoryId)
    {
        var result = await sender.Send(new UpdateCategoryCommand{CategoryId = categoryId, CategoryUpdateModel = categoryModel});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> DeleteCategory(ISender sender, [Required] Guid categoryId)
    {
        var result = await sender.Send(new DeleteCategoryCommand{CategoryId = categoryId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}