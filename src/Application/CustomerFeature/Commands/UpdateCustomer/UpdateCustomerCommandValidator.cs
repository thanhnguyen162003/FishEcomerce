using Application.Common.Models.CustomerModels;

namespace Application.CustomerFeature.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<CustomerUpdateModel>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Birthday)
            .Must(birthday => birthday > new DateOnly(1900, 1, 1))
            .WithMessage("BirthDate must be after January 1, 1900")
            .When(x => x.Birthday is not null);
        
        RuleFor(x => x.Phone)
            .Matches(@"^0\d{9,10}$").WithMessage("Phone number must start with 0 and contain 10 or 11 digits.")
            .When(x=> x.Phone is not null);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.");
    }
}