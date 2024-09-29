using Application.Common.Models.BreedModels;

namespace Application.Products.Commands.BreedModels.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<BreedCreateRequestModel>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Breed Name is required.")
            .MinimumLength(3).WithMessage("Breed Name must at least 3 character")
            .MaximumLength(255).WithMessage("Breed Name must not exceed 150 characters.");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Breed description is required.")
            .MinimumLength(3).WithMessage("Breed description must at least 3 character");
    }
}