using Application.Common.Models.StaffModels;

namespace Application.Staffs.Commands.UpdateStaff;

public class UpdateStaffCommandValidator : AbstractValidator<StaffUpdateModel>
{
    public UpdateStaffCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}