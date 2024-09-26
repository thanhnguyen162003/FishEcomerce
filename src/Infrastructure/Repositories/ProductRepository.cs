using FishEcomerce.Domain.Entities;
using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;

namespace Microsoft.Extensions.DependencyInjection.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(KingFishContext context) : base(context)
    {
    }
}
