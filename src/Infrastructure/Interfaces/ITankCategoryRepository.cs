using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface ITankCategoryRepository : IRepository<TankCategory>
{
    Task<List<TankCategory>> GetTankCategoriesByIdAsync(IEnumerable<Guid> categoriesIds);
}