using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.CreateFishProduct;

public class CreateFishProductCommandValidator : AbstractValidator<FishProductCreateModel>
{
    private readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];
    
    public CreateFishProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters");
        
        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity must be larger than or equal to 0");
        
        RuleFor(x => x.Price)
            .Must((x,price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice");
        
        RuleFor(x => x.OriginalPrice)
            .GreaterThan(0).WithMessage("Original Price must be positive");

        RuleFor(x => x.FishModel)
            .NotNull().WithMessage("Fish model is required")
            .ChildRules(fish =>
            {
                fish.RuleFor(f => f.Size)
                    .Must(quantity => quantity > 0).WithMessage("Size must be positive")
                    .NotEmpty().WithMessage("Size is required");
        
                fish.RuleFor(f => f.Age)
                    .Must(quantity => quantity > 0).WithMessage("Age must be positive")
                    .NotEmpty().WithMessage("Age is required");

                fish.RuleFor(f => f.Origin)
                    .NotEmpty().WithMessage("Origin information is required");

                fish.RuleFor(f => f.FoodAmount)
                    .Must(quantity => quantity > 0).WithMessage("Food amount must be positive")
                    .NotEmpty().WithMessage("Food amount is required");

                fish.RuleFor(f => f.Weight)
                    .Must(quantity => quantity > 0).WithMessage("Weight must be positive")
                    .NotEmpty().WithMessage("Weight is required");

                fish.RuleFor(f => f.Health)
                    .NotEmpty().WithMessage("Health information is required");
            });

        RuleForEach(x => x.FishAward).ChildRules(award =>
        {
            award.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            award.RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            award.RuleFor(x => x.AwardDate).NotEmpty().WithMessage("AwardDate is required");
        })
        .When(x => x.FishAward != null && x.FishAward.Any());
        
        RuleForEach(x => x.ImageFiles).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        });
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedExtensions.Contains(extension);
    }
}