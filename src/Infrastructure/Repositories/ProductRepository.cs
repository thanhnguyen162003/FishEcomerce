using Domain.Constants;
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
            .Include(x => x.Tank.Categories)
            .Include(x => x.Images)
            .Include(x => x.Supplier)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == productId && x.DeletedAt == null);
            
    }

    public async Task<IEnumerable<Product>> GetAllProductIncludeFish()
    {
        return await Entities
            .Include(x => x.Fish)
            .Include(x => x.Images)
            .Include(x => x.Supplier)
            .Where(x => x.DeletedAt == null
            && x.Type == TypeConstant.FISH
            ).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductIncludeTank()
    {
        return await Entities
            .Include(x => x.Tank)
            .Include(x => x.Images)
            .Include(x => x.Supplier)
            .Where(x => x.DeletedAt == null
                        && x.Type == TypeConstant.TANK
            ).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductById(Guid Id)
    {
        var product = await Entities
            .Where(x => x.Id.Equals(Id))
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            .ToListAsync();
        return product;
    }
}