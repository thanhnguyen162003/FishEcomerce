using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<ProductCreateModel>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        
        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be larger than or equal to 0");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .IsInEnum().WithMessage("Type must be a valid enum");
        
        RuleFor(x => x.OriginalPrice)
            .NotEmpty().WithMessage("Original Price is required")
            .GreaterThan(0).WithMessage("Original Price must be positive");
        
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .Must((x,price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice");
        
        RuleFor(x => x.ImageFiles).NotEmpty().WithMessage("ImageFiles is required");
        
        RuleForEach(x => x.ImageFiles).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        });
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
}