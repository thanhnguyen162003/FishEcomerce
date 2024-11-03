using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IBreedRepository : IRepository<Breed>
{
    Task<Breed?> GetBreedById(Guid breedId);
}