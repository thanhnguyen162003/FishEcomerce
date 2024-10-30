﻿using System.Data;
using Application.Common.Models.ProductModels;

namespace Application.Products.Commands.UpdateTankProduct;

public class UpdateTankProductCommandValidator : AbstractValidator<TankProductUpdateModel>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public UpdateTankProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage("Name is required")
            .When(x => x.Name is not null);
        
        RuleFor(x => x.StockQuantity)
            .Must(quantity => quantity > 0).WithMessage("Stock quantity must be positive")
            .When(x => x.StockQuantity is not null);
        
        RuleFor(x => x.Price)
            .Must((x,price) => price > x.OriginalPrice).WithMessage("Price must be greater than OriginalPrice")
            .When(x => x.Price is not null);
        
        RuleFor(x => x.OriginalPrice)
            .Must(orginalPrice => orginalPrice > 0).WithMessage("Original Price must be positive")
            .When(x => x.OriginalPrice is not null);

        RuleForEach(x => x.UpdateImages).ChildRules(file =>
        {
            file.RuleFor(x => x.Length).GreaterThan(0).WithMessage("File is empty");
            file.RuleFor(x => x.FileName).Must(HasAllowedExtension).WithMessage("File extension is not allowed");
        }).When(x => x.UpdateImages is not null && x.UpdateImages.Any());
        
        RuleFor(x => x.TankModel).ChildRules(x =>
        {
            x.RuleFor(x => x.Size)
                .NotEmpty().WithMessage("Size is required.")
                .When(x => x.Size is not null);
            
            x.RuleFor(x => x.SizeInformation)
                .NotEmpty().WithMessage("Size Information is required.")
                .When(x => x.SizeInformation is not null);
            
            x.RuleFor(x => x.GlassType)
                .NotEmpty().WithMessage("Glass Type is required.")
                .When(x => x.GlassType is not null);
            
        }).When(x => x.TankModel is not null);
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return _allowedExtensions.Contains(extension);
    }
}