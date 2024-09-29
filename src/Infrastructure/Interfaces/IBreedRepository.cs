using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IBreedRepository : IRepository<Breed>
{
    Task<IEnumerable<Breed>> GetBreedByName(string? search, int pageSize, int pageNumber);
}