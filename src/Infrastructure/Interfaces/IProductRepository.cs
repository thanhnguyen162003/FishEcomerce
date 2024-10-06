using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductIncludeTankById(Guid productId);
    Task<Product> GetProductIncludeImageById(Guid productId);
    Task<IEnumerable<Product>> GetAllProductIncludeFish();
    Task<IEnumerable<Product>> GetAllProductIncludeTank();
}