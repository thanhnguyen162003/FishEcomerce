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
            .Include(x => x.Tank.TankCategories)
            .Include(x => x.Images)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == productId && x.DeletedAt == null);
    }

    public async Task<Product?> GetProductIncludeFishById(Guid productId)
    {
        return await Entities
            .Include(x => x.Fish)
            .Include(x => x.Fish.Breed)
            .Include(x => x.Fish.Awards)
            .Include(x => x.Images)
            .Include(x => x.Feedbacks)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == productId && x.DeletedAt == null);
    }

    public IQueryable<Product> GetAllProductIncludeFish()
    {
        return Entities
            .Where(x => x.DeletedAt == null && x.Type == TypeConstant.FISH)
            .Include(x => x.Fish)
            .Include(x => x.Fish.Breed)
            .Include(x => x.Fish.Awards)
            .Include(x => x.Images)
            .Include(x => x.Feedbacks)
            .Include(x => x.Staff)
            .AsSplitQuery()
            .AsQueryable();
    }

    public async Task<Product?> GetProductIncludeImageById(Guid productId)
    {
        return await Entities.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == productId && x.DeletedAt == null);
    }

    public async Task<decimal?> GetProductPrice(Guid productId)
    {
        return await Entities.Where(x => x.Id == productId).Select(x => x.Price).FirstOrDefaultAsync();
    }

    public async Task<List<Product>> GetProductsByOrderDetailIds(IEnumerable<Guid?> productIds)
    {
        return await Entities.Where(x => productIds.Contains(x.Id)).ToListAsync();
    }

    public async Task<List<Product>> SearchProducts(string name, string type, CancellationToken cancellationToken)
    {
        var property = typeof(Product).GetProperty(type);
        if (property is null || !property.PropertyType.IsClass)
        {
            return [];
        }
        
        return await Entities
            .AsNoTracking()
            .Include(property.Name)
            .Where(x =>  x.Name.ToLower().Contains(name) && x.Type == type.ToLower())
            .OrderByDescending(x => x.CreatedAt)
            .Take(6)
            .ToListAsync(cancellationToken);
    }
}