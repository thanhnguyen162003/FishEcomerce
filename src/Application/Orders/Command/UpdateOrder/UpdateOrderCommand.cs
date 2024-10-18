using System.Net;
using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.UoW;

namespace Application.Orders.Command.UpdateOrder;

public record UpdateOrderCommand : IRequest<ResponseModel>
{
    public Guid OrderId { get; init; }
    public OrderUpdateModel OrderUpdateModel { get; init; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Order not found");
        }

        if (request.OrderUpdateModel.OrderStatus is not null)
        {
            order.Status = request.OrderUpdateModel.OrderStatus.ToString();
        }
        
        order.ShippedDate = request.OrderUpdateModel.ShippedDate ?? order.ShippedDate;
        order.UpdatedAt = DateTime.Now;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Order successfully updated");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}