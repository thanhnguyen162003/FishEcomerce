using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductIncludeTankById(Guid productId);
    Task<Product> GetProductIncludeImageById(Guid productId);
    Task<Product?> GetProductIncludeFishById(Guid productId);
    Task<IQueryable<Product>> GetAllProductIncludeFish();
    Task<IEnumerable<Product>> GetAllProductIncludeTank();
    Task<decimal?> GetProductPrice(Guid productId);
    Task<List<Product>> GetProductsByOrderDetailIds(IEnumerable<Guid?> productIds);
}