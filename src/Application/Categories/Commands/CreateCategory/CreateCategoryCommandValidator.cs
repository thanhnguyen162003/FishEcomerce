using Application.Common.Models.CategoryModels;

namespace Application.TankCategories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CategoryCreateModel>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level cannot be empty");
        
        RuleFor(x => x.TankType)
            .NotEmpty().WithMessage("Tank type cannot be empty");
    }   
}