using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Admins.BanCustomer;
using Application.Admins.CreateStaff;
using Application.Admins.DeleteStaff;
using Application.Admins.UpdateAdmin;
using Application.Common.Models.StaffModels;
using Application.Common.Utils;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class AdminEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/admin");
        group.MapPost("staff", CreateStaff).RequireAuthorization("Admin").WithName(nameof(CreateStaff));
        group.MapDelete("customer/{customerId}", BanCustomer).RequireAuthorization("Admin").WithName(nameof(BanCustomer));
        group.MapDelete("staff/{staffId}", DeleteStaff).RequireAuthorization("Admin").WithName(nameof(DeleteStaff));
        group.MapPatch("staff/{staffId}", UpdateStaffToAdmin).RequireAuthorization("Admin").WithName(nameof(UpdateStaffToAdmin));
    }
    
    private async Task<IResult> CreateStaff(ISender sender, [FromBody, Required] StaffCreateModel model,
        ValidationHelper<StaffCreateModel> validationHelper)
    {
        var (isValid, response) = await validationHelper.ValidateAsync(model);
        if (!isValid)
        {
            return Results.BadRequest(response);
        }
        
        var result = await sender.Send(new CreateStaffCommand(){StaffCreateModel = model});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> BanCustomer(ISender sender, Guid customerId)
    {
        var result = await sender.Send(new BanCustomerCommand() { CustomerId = customerId });
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    private async Task<IResult> DeleteStaff(ISender sender, Guid staffId)
    {
        var result = await sender.Send(new DeleteStaffCommand(){StaffId = staffId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }

    private async Task<IResult> UpdateStaffToAdmin(ISender sender, Guid staffId)
    {
        var result = await sender.Send(new UpdateStaffToAdminCommand(){StaffId = staffId});
        return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
    }
}