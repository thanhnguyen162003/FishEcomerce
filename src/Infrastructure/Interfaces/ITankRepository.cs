using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface ITankRepository : IRepository<Tank>
{
    Task<Tank?> GetTankById(Tank tank);
}