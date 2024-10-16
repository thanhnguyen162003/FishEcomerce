using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(KingFishDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetOrderByOrderCode(long orderCode)
    {
        return await Entities.Include(x=>x.OrderDetails).AsSplitQuery().FirstOrDefaultAsync(x => x.OrderCode==orderCode);
    }
}