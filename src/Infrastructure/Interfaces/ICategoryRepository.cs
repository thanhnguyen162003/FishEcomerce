using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> ExistsByName(string name);
    Task<List<Category>> GetCategoriesByIdAsync(IEnumerable<Guid> categoriesIds);

}