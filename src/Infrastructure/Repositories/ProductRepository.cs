using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(KingFishDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetProductIncludeTankById(Guid productId)
    {
        return await Entities
            .Include(x => x.Tank)
            .Include(x => x.Images)
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.Id == productId && x.DeletedAt == null);
            
    }
}