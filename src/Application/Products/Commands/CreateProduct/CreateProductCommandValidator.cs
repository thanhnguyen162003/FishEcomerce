namespace Microsoft.Extensions.DependencyInjection.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        // example but not use yet
        RuleFor(x => x.Model!.Name)
            .MaximumLength(255)
            .NotEmpty();
    }
}
