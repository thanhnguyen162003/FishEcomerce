using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.Utils;
using Application.CustomerFeature.Commands.UpdateCustomer;
using Application.CustomerFeature.Queries;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Endpoints
{
    public class CustomerEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/v1/customers");
            group.MapPatch("profile", UpdateCustomer).RequireAuthorization("Customer").WithName(nameof(UpdateCustomer));
            group.MapGet("profile", GetCustomer).RequireAuthorization("Customer").WithName(nameof(GetCustomer));
            group.MapGet("",GetCustomers).RequireAuthorization("Admin&Staff").WithName(nameof(GetCustomers));
        }
        
        private async Task<IResult> UpdateCustomer(ISender sender, [FromBody, Required] CustomerUpdateModel customerModel, ValidationHelper<CustomerUpdateModel> validationHelper)
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
        
        private async Task<IResult> GetCustomers(ISender sender, [AsParameters] CustomerQueryFilter filter, HttpContext httpContext)
        {
            filter.ApplyDefaults();
            var result = await sender.Send(new GetCustomersQuery(){QueryFilter = filter});
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
}
