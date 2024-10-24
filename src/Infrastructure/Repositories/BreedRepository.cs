using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BreedRepository : Repository<Breed>, IBreedRepository
{
    public BreedRepository(KingFishDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Breed>> GetBreedByName(string? search, int pageSize, int pageNumber)
    {
        var breed = Entities
            .Where(x => x.Name.ToLower().Contains(search))
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            .ToList();
        return breed;
    }
    public async Task<Breed?> GetBreedById(Guid breedId)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Id == breedId);
    }
}