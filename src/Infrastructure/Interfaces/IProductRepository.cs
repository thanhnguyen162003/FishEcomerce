using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetProductIncludeTankById(Guid productId);
    Task<Product?> GetProductIncludeImageById(Guid productId);
    Task<Product?> GetProductIncludeFishById(Guid productId);
    IQueryable<Product> GetAllProductIncludeFish();
    Task<decimal?> GetProductPrice(Guid productId);
    Task<List<Product>> GetProductsByOrderDetailIds(IEnumerable<Guid?> productIds);
    Task<List<Product>> SearchProducts(string name, string type, CancellationToken cancellationToken);
    Task<Product?> GetProductById(Guid productId);
}