using Application.Common.Models.FishAwardModels;

namespace Application.FishAwards.Commands.CreateFishAward;

public class CreateFishAwardCommandValidator : AbstractValidator<FishAwardCreateRequestModel>
{
    public CreateFishAwardCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Award Name is required.")
            .MinimumLength(3).WithMessage("Award Name must at least 3 character")
            .MaximumLength(255).WithMessage("Award Name must not exceed 150 characters.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Award description is required.")
            .MinimumLength(3).WithMessage("Award description must at least 3 character");
        RuleFor(v => v.AwardDate)
            .Must(awardDate => awardDate.CompareTo(DateTime.Today.Date) < 0
                            && awardDate.CompareTo(DateTime.Today.AddYears(-25).Date) > 0)
            .WithMessage("The AwardDate must be less than today and not less than 25 years ago.");
    }
}