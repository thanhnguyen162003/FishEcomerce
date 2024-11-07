using Application.Common.Models.TankCategoryModels;

namespace Application.TankCategories.Commands.CreateTankCategory;

public class CreateTankCategoryCommandValidator : AbstractValidator<TankCategoryCreateModel>
{
    public CreateTankCategoryCommandValidator()
    {
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level cannot be empty");
        
        RuleFor(x => x.TankType)
            .NotEmpty().WithMessage("Tank type cannot be empty");
    }   
}