using System.Net;
using Application.Auth;
using Application.Auth.Commands.UpdatePassword;
using Application.Common.Models;
using Application.Common.Utils;

namespace Web.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            
            endpoints.MapPost("/api/auth/register-customer", async (RegisterCustomerRequest request, IAuthService authService) =>
            {
                var result = await authService.RegisterCustomer(request.Username, request.Password, request.Name, request.Phone);
                return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
            }).WithRequestValidation<RegisterCustomerRequest>();

            endpoints.MapPost("/api/auth/login-customer", async (LoginCustomerRequest request, IAuthService authService) =>
            {
                var result = await authService.LoginCustomer(request.Username, request.Password);
                return result.Status == HttpStatusCode.OK ? Results.Ok(new {Token = result.Data}) : Results.BadRequest(result.Message);
            });

            endpoints.MapPost("/api/auth/login-staff", async (LoginStaffRequest request, IAuthService authService) =>
            {
                var result = await authService.LoginStaff(request.Username, request.Password);
                return result.Status == HttpStatusCode.OK ? Results.Ok(new {Token = result.Data}) : Results.BadRequest(result);
            });

            endpoints.MapPatch("/api/password",
                async (PasswordUpdateModel request, ISender sender,
                    ValidationHelper<PasswordUpdateModel> validationHelper) =>
                {
                    var (isValid, response) = await validationHelper.ValidateAsync(request);
                    if (!isValid)
                    {
                        return Results.BadRequest(response);
                    }
        
                    var result = await sender.Send(new UpdatePasswordCommand(){Role = "Staff", PasswordUpdateModel = request });
                    return result.Status == HttpStatusCode.OK ? Results.Ok(result) : Results.BadRequest(result);
                }).RequireAuthorization();
        }

        public record RegisterCustomerRequest(string Username ,string Password, string Name, string Phone);

        public class RegisterCustomerRequestValidator : AbstractValidator<RegisterCustomerRequest>
        {
            public RegisterCustomerRequestValidator()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
                
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                    .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter")
                    .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter")
                    .Matches(@"[#$%!@&^*]+").WithMessage("Password must contain at least one special character (#$%!@&^*)");
                
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

                RuleFor(x => x.Phone)
                    .Matches(@"^0\d{9,10}$").WithMessage("Phone number must start with 0 and contain 10 or 11 digits.");
            }
        }
        
        private record LoginCustomerRequest(string Username, string Password);
        private record LoginStaffRequest(string Username, string Password);
    }
}
