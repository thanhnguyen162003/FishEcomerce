using System.ComponentModel.DataAnnotations;
using Application.CustomerFeature.Models;
using Application.CustomerFeature.Services;
using Application.CustomerFeature.Models;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints
{
    public class CustomerEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/v1/customers");

            // Các endpoint cho CRUD khách hàng
            group.MapPost("", CreateCustomer).WithName(nameof(CreateCustomer));
            group.MapPut("{id:guid}", UpdateCustomer).WithName(nameof(UpdateCustomer));
            group.MapDelete("{id:guid}", DeleteCustomer).WithName(nameof(DeleteCustomer));
            group.MapGet("", GetAllCustomers).WithName(nameof(GetAllCustomers));
        }

        // Tạo khách hàng mới
        public async Task<IResult> CreateCustomer([FromServices] CustomerService customerService, [FromBody, Required] CustomerCreateModel customerModel)
        {
            await customerService.AddCustomerAsync(customerModel);
            return Results.Ok("Customer created successfully");
        }

        // Cập nhật thông tin khách hàng
        public async Task<IResult> UpdateCustomer([FromServices] CustomerService customerService, Guid id, [FromBody, Required] CustomerUpdateModel customerModel)
        {
            await customerService.UpdateCustomerAsync(id, customerModel);
            return Results.Ok("Customer updated successfully");
        }

        // Xóa khách hàng
        public async Task<IResult> DeleteCustomer([FromServices] CustomerService customerService, Guid id)
        {
            await customerService.DeleteCustomerAsync(id);
            return Results.Ok("Customer deleted successfully");
        }

        // Lấy tất cả khách hàng
        public async Task<IResult> GetAllCustomers([FromServices] CustomerService customerService)
        {
            var customers = await customerService.GetAllCustomersAsync();
            return Results.Ok(customers);
        }
    }
}
