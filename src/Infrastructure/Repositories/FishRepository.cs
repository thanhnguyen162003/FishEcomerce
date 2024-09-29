using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FishRepository : Repository<Fish>, IFishRepository
{
    public FishRepository(KingFishDbContext context) : base(context)
    {
    }
}