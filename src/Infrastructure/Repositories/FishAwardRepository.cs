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
}