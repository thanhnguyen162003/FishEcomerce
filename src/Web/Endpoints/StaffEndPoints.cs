using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Auth.Commands.UpdatePassword;
using Application.Common.Models;
using Application.Common.Models.StaffModels;
using Application.Common.Utils;
using Application.Staffs.Commands.UpdateStaff;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class StaffEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/staff");
        group.MapPatch("", UpdateStaff).RequireAuthorization("Staff").WithName(nameof(UpdateStaff));
    }

    private async Task<IResult> UpdateStaff(ISender sender, [FromBody, Required] StaffUpdateModel model,
        ValidationHelper<StaffUpdateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(model);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new UpdateStaffCommand(){ StaffUpdateModel = model });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}