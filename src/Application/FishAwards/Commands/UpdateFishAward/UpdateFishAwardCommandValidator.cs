using Application.Common.Models.FishAwardModels;

namespace Application.FishAwards.Commands.UpdateFishAward;

public class UpdateFishAwardCommandValidator : AbstractValidator<FishAwardUpdateRequestModel>
{
    public UpdateFishAwardCommandValidator()
    {
        RuleFor(v => v.Name)
             .NotEmpty().WithMessage("Breed Name is required.")
             .MinimumLength(3).WithMessage("Breed Name must at least 3 character")
             .MaximumLength(255).WithMessage("Breed Name must not exceed 150 characters.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Breed description is required.")
            .MinimumLength(3).WithMessage("Breed description must at least 3 character");
        RuleFor(v => v.AwardDate)
            .Must(awardDate => awardDate.CompareTo(DateTime.Today.Date) < 0
                            && awardDate.CompareTo(DateTime.Today.AddYears(-25).Date) > 0)
            .WithMessage("The AwardDate must be less than today and not less than 25 years ago.");
    }
}