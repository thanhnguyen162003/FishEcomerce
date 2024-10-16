using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderByOrderCode(long orderCode);
    Task<Order?> GetOrderByOrderIdAndCustomerId(Guid orderId, Guid customerId);
}