using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.CreateTankProduct;

public class CreateTankProductCommandValidator : AbstractValidator<TankProductCreateModel>
{
    public CreateTankProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required");
        
        RuleFor(x => x.StockQuantity)
            .Must(quantity => quantity > 0).WithMessage("Stock quantity must be positive");
        
        RuleFor(x => x.Price)
            .Must((x,price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice");
        
        RuleFor(x => x.OriginalPrice)
            .Must(orginalPrice => orginalPrice > 0).WithMessage("Original Price must be positive");

        RuleFor(x => x.TankModel)
            .NotNull().WithMessage("Tank model is required");
        
        RuleFor(x => x.TankModel.Size)
            .NotEmpty().WithMessage("Tank model size is required");
        
        RuleFor(x => x.TankModel.GlassType)
            .NotEmpty().WithMessage("Tank model glass type is required");
        
        RuleFor(x => x.TankModel.SizeInformation)
            .NotEmpty().WithMessage("Tank model size information is required");
    }
}