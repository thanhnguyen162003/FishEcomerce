using Application.Common.Models.CategoryModels;

namespace Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<CategoryUpdateModel>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .When(x => x.Name is not null);
        
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type is invalid")
            .When(x => x.Type is not null);
    }
}