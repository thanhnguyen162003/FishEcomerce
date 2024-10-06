using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
    public ImageRepository(KingFishDbContext context) : base(context)
    {
    }

    public async Task<List<Image>> GetImagesByIdAsync(IEnumerable<Guid> imagesIds)
    {
        return await Entities.Where(x => imagesIds.Contains(x.Id)).ToListAsync();
    }
}