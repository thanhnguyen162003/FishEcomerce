using Application.Auth;

namespace Web.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            // Endpoint cho đăng ký Customer
            endpoints.MapPost("/api/auth/register-customer", async (RegisterCustomerRequest request, IAuthService authService) =>
            {
                var result = await authService.RegisterCustomer(request.Email, request.Password, request.Name);
                if (!result)
                {
                    return Results.BadRequest("Customer already exists");
                }
                return Results.Ok("Customer registered successfully");
            });

            // Endpoint cho đăng ký Supplier
            endpoints.MapPost("/api/auth/register-supplier", async (RegisterSupplierRequest request, IAuthService authService) =>
            {
                var result = await authService.RegisterSupplier(request.Username, request.Password, request.CompanyName);
                if (!result)
                {
                    return Results.BadRequest("Supplier already exists");
                }
                return Results.Ok("Supplier registered successfully");
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
