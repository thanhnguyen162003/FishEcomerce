using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IFishAwardRepository : IRepository<FishAward>
{
    Task<List<FishAward>> GetAwardByIdAsync(IEnumerable<Guid> awardIds);
}