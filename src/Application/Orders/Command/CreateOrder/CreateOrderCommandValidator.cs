using Application.Common.Models.OrderModels;

namespace Application.Orders.Command.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<OrderCreateModel>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .IsInEnum().WithMessage("Payment method must be a valid enum");

        RuleFor(x => x.ShipAddress)
            .NotEmpty().WithMessage("Ship address is required");

        RuleFor(x => x.OrderDetails)
            .Must(orderDetails => orderDetails.Count > 0).WithMessage("Order details are required");
    }
}