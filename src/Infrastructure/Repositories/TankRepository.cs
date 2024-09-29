using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class TankRepository : Repository<Tank> , ITankRepository
{
    public TankRepository(KingFishDbContext context) : base(context)
    {
    }
}