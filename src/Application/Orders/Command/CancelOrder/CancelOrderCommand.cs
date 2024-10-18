using System.Net;
using Application.Common.Models;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Enums;

namespace Application.Orders.Command.CancelOrder;

public record CancelOrderCommand : IRequest<ResponseModel>
{
    public Guid OrderId { get; init; }
}

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var customerId = _claimsService.GetCurrentUserId;
        var order = await _unitOfWork.OrderRepository.GetOrderByOrderIdAndCustomerId(request.OrderId, customerId);

        if (order is null)
        {
            return new ResponseModel(HttpStatusCode.Forbidden, "You dont have permission to cancel this order.");
        }

        if (!order.Status.Equals(OrderStatus.Pending.ToString()) || !order.Status.Equals(OrderStatus.Packaging.ToString()))
        {
            return new ResponseModel(HttpStatusCode.Forbidden, "Your order is on delivery.");
        }
        
        order.Status = OrderStatus.Cancel.ToString();
        order.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Order cancelled.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}