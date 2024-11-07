using Application.Common.Models.CategoryModels;

namespace Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CategoryCreateModel>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .IsInEnum().WithMessage("Type is invalid.");
    }
}