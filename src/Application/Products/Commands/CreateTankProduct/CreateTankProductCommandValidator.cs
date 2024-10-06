using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.CreateTankProduct;

public class CreateTankProductCommandValidator : AbstractValidator<TankProductCreateModel>
{
    private readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];

    public CreateTankProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        
        RuleFor(x => x.StockQuantity)
            .GreaterThan(0).WithMessage("Stock quantity must be positive");
        
        RuleFor(x => x.Price)
            .Must((x,price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice");
        
        RuleFor(x => x.OriginalPrice)
            .GreaterThan(0).WithMessage("Original Price must be positive");

        RuleFor(x => x.ImageFiles).NotEmpty().WithMessage("ImageFiles is required");

        RuleFor(x => x.CategoriesIds).NotEmpty().WithMessage("CategoriesIds is required");
        
        RuleForEach(x => x.ImageFiles).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        });
        
        RuleFor(x => x.TankModel).NotNull().WithMessage("Tank model is required")
            .ChildRules(tank =>
            {
                tank.RuleFor(t => t.Size)
                    .NotEmpty().WithMessage("Tank model size is required");
        
                tank.RuleFor(t => t.GlassType)
                    .NotEmpty().WithMessage("Tank model glass type is required");
        
                tank.RuleFor(t => t.SizeInformation)
                    .NotEmpty().WithMessage("Tank model size information is required");
            });
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedExtensions.Contains(extension);
    }
}