using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(KingFishDbContext context) : base(context)
    {
    }
    
    public async Task<List<Category>> GetCategoriesByIdAsync(IEnumerable<Guid> categoriesIds)
    {
        return await Entities.Where(x => categoriesIds.Contains(x.Id)).ToListAsync();
    }
    
}