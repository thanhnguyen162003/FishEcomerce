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
    public async Task<IEnumerable<Breed>> GetBreedById(Guid Id)
    {
        var breed = Entities
            .Where(x => x.Id.Equals(Id))
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            .ToList();
        return breed;
    }
}