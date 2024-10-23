using Application.Common.Models;

namespace Application.Auth.Commands.UpdatePassword;

public class UpdatePasswordCommandValidator : AbstractValidator<PasswordUpdateModel>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[#$%!@&^*]+").WithMessage("Password must contain at least one special character (#$%!@&^*)");
    }
}