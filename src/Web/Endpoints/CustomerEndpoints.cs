using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models.CustomerModels;
using Application.Common.Utils;
using Application.CustomerFeature.Commands.UpdateCustomer;
using Application.CustomerFeature.Queries;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints
{
    public class CustomerEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/v1/customers");
            group.MapPatch("{customerId}", UpdateCustomer).RequireAuthorization("Customer").WithName(nameof(UpdateCustomer));
            group.MapGet("", GetCustomer).RequireAuthorization("Customer").WithName(nameof(GetCustomer));
        }
        
        private async Task<IResult> UpdateCustomer(ISender sender, Guid id, [FromBody, Required] CustomerUpdateModel customerModel, ValidationHelper<CustomerUpdateModel> validationHelper)
        {
            var (isValid, response) = await validationHelper.ValidateAsync(customerModel);
            if (!isValid)
            {
                return Results.BadRequest(response);
            }
            
            var result = await sender.Send(new UpdateCustomerCommand() { CustomerUpdateModel = customerModel });
            return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
        }

        private async Task<IResult> GetCustomer(ISender sender)
        {
            var result = await sender.Send(new GetCustomerByIdQuery());
            return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
        }
    }
}
