using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetCategoriesByIdAsync(IEnumerable<Guid> categoriesIds);
}