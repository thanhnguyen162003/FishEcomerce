using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<ProductUpdateModel>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithName("Name is required")
            .MaximumLength(255).WithMessage("Name is required")
            .When(x => x.Name is not null);
        
        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be larger than or equal to 0")
            .When(x => x.StockQuantity is not null);
        
        RuleForEach(x => x.UpdateImages).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        }).When(x => x.UpdateImages is not null && x.UpdateImages.Any());
        
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type is invalid")
            .When(x => x.Type is not null);
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
}