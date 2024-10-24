using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FishAwardRepository : Repository<FishAward>, IFishAwardRepository
{
    public FishAwardRepository(KingFishDbContext context) : base(context)
    {
    }
    public async Task<List<FishAward>> GetAwardByIdAsync(IEnumerable<Guid> awardIds)
    {
        return await Entities.Where(x => awardIds.Contains(x.Id)).ToListAsync();
    }
}