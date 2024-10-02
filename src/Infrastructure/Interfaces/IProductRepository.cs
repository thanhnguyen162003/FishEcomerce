using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductIncludeTankById(Guid productId);
    Task<IEnumerable<Product>> GetProductById(Guid Id);
    Task<IEnumerable<Product>> GetAllProductIncludeFish();
}