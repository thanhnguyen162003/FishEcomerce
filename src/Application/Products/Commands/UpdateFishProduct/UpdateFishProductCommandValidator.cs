using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.UpdateFishProduct;

public class UpdateFishProductCommandValidator : AbstractValidator<FishProductUpdateModel>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public UpdateFishProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters")
            .When(x => x.Name is not null);

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be larger than or equal to 0")
            .When(x => x.StockQuantity is not null);

        RuleForEach(x => x.UpdateImages).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        }).When(x => x.UpdateImages is not null && x.UpdateImages.Any());

        RuleFor(x => x.FishModel).ChildRules(x =>
        {
            x.RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Fish size must be positive")
                .When(x => x.Size is not null);
            
            x.RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Fish age must be positive")
                .When(x => x.Age is not null);
            
            x.RuleFor(x => x.Origin)
                .NotEmpty().WithMessage("Fish origin is required")
                .When(x => x.Origin is not null);
            
            x.RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Fish weight must be positive")
                .When(x => x.Weight is not null);
            
            x.RuleFor(x => x.FoodAmount)
                .GreaterThan(0).WithMessage("Fish amount must be positive")
                .When(x => x.FoodAmount is not null);
                
            x.RuleFor(x => x.Health)
                .NotEmpty().WithMessage("Fish health is required")
                .When(x => x.Health is not null);
        });
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
}