using Application.Common.Models.OrderModels;

namespace Application.Orders.Command.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<OrderUpdateModel>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.OrderStatus)
            .IsInEnum().WithMessage("Order status must be a valid enum")
            .When(x => x.OrderStatus is not null);
    }
}