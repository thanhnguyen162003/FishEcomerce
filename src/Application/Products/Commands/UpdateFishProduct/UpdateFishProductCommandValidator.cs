using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.UpdateFishProduct;

public class UpdateFishProductCommandValidator : AbstractValidator<FishProductUpdateModel>
{
    public UpdateFishProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");

        RuleFor(x => x.StockQuantity)
            .Must(quantity => quantity > 0).WithMessage("Stock quantity must be positive");

        RuleFor(x => x.Price)
            .Must((x, price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice");

        RuleFor(x => x.OriginalPrice)
            .Must(orginalPrice => orginalPrice > 0).WithMessage("Original Price must be positive");

        RuleFor(x => x.FishModel)
            .NotNull().WithMessage("Fish model is required");

        RuleFor(x => x.FishModel.Size)
            .Must(quantity => quantity > 0).WithMessage("Size must be positive")
            .NotEmpty().WithMessage("Size is required");

        RuleFor(x => x.FishModel.Age)
            .Must(quantity => quantity > 0).WithMessage("Age must be positive")
            .NotEmpty().WithMessage("Age is required");

        RuleFor(x => x.FishModel.Origin)
            .NotEmpty().WithMessage("Origin information is required");

        RuleFor(x => x.FishModel.FoodAmount)
            .Must(quantity => quantity > 0).WithMessage("Age must be positive")
            .NotEmpty().WithMessage("Age is required");

        RuleFor(x => x.FishModel.Weight)
            .Must(quantity => quantity > 0).WithMessage("Weight must be positive")
            .NotEmpty().WithMessage("Weight is required");

        RuleFor(x => x.FishModel.Health)
           .NotEmpty().WithMessage("Origin information is required");
    }
}