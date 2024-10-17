using Application.Auth;

namespace Web.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Endpoint cho đăng ký Customer
            //endpoints.MapPost("/api/auth/register-customer", async (RegisterCustomerRequest request, IAuthService authService) =>
            //{
            //    var result = await authService.RegisterCustomer(request.Email, request.Password, request.Name);
            //    if (!result)
            //    {
            //        return Results.BadRequest("Customer already exists");
            //    }
            //    return Results.Ok("Customer registered successfully");
            //});
            endpoints.MapPost("/api/auth/register-customer", async (RegisterCustomerRequest request, IAuthService authService) =>
            {
                try
                {
                    var result = await authService.RegisterCustomer(request.Email, request.Password, request.Name);
                    return Results.Ok("Customer registered successfully");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Endpoint cho đăng ký Supplier
            endpoints.MapPost("/api/auth/register-supplier", async (RegisterSupplierRequest request, IAuthService authService) =>
            {
                try
                {
                    var result = await authService.RegisterSupplier(request.Username, request.Password, request.CompanyName);
                    return Results.Ok("Supplier registered successfully");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            // Endpoint cho đăng nhập Customer
            endpoints.MapPost("/api/auth/login-customer", async (LoginCustomerRequest request, IAuthService authService) =>
            {
                var token = await authService.LoginCustomer(request.Email, request.Password);
                if (token == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(new { Token = token });
            });


            // Endpoint cho đăng nhập Supplier
            endpoints.MapPost("/api/auth/login-supplier", async (LoginSupplierRequest request, IAuthService authService) =>
            {
                var token = await authService.LoginSupplier(request.Username, request.Password);
                if (token == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(new { Token = token });
            });
        }

        public record RegisterCustomerRequest(string Email, string Password, string Name);
        public record RegisterSupplierRequest(string Username, string Password, string CompanyName);
        public record LoginCustomerRequest(string Email, string Password);
        public record LoginSupplierRequest(string Username, string Password);
    }
}
