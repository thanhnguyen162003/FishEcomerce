using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TankCategoryRepository : Repository<TankCategory>, ITankCategoryRepository
{
    public TankCategoryRepository(KingFishDbContext context) : base(context)
    {
    }
    
    public async Task<List<TankCategory>> GetTankCategoriesByIdAsync(IEnumerable<Guid> categoriesIds)
    {
        return await Entities.Where(x => categoriesIds.Contains(x.Id)).ToListAsync();
    }
}