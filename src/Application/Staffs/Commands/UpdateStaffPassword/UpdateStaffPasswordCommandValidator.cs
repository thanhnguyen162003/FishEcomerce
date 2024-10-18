using Application.Common.Models.StaffModels;

namespace Application.Staffs.Commands.UpdateStaffPassword;

public class UpdateStaffPasswordCommandValidator : AbstractValidator<StaffPasswordUpdateModel>
{
    public UpdateStaffPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[#$%!@&^*]+").WithMessage("Password must contain at least one special character (#$%!@&^*)");
        
        RuleFor(x => x.ConfirmPassword)
            .Must((x, password) => password.Equals(x.NewPassword)).WithMessage("Passwords do not match");
    }
}