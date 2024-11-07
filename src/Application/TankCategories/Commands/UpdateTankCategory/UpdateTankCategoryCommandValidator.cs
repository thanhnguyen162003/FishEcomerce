using Application.Common.Models.TankCategoryModels;

namespace Application.TankCategories.Commands.UpdateTankCategory;

public class UpdateTankCategoryCommandValidator : AbstractValidator<TankCategoryUpdateModel>
{
    public UpdateTankCategoryCommandValidator()
    {
        RuleFor(x => x.TankType)
            .NotEmpty().WithMessage("Tank type cannot be empty")
            .When(x => x.TankType is not null);
        
        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level cannot be empty")
            .When(x => x.Level is not null);
    }
}