using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductIncludeTankById(Guid productId);
}