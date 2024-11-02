using Application.Common.Models.StaffModels;

namespace Application.Admins.Commands.CreateStaff;

public class CreateStaffCommandValidator : AbstractValidator<StaffCreateModel>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        
        RuleFor(x=> x.FullName).NotEmpty().WithMessage("Full name is required");
    }
}