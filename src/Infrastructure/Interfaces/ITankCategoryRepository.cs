using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface ITankCategoryRepository : IRepository<TankCategory>
{
    Task<List<TankCategory>> GetCategoriesByIdAsync(IEnumerable<Guid> categoriesIds);
}