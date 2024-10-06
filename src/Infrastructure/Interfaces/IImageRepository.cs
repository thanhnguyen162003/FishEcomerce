using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IImageRepository : IRepository<Image>
{
    Task<List<Image>> GetImagesByIdAsync(IEnumerable<Guid> imagesIds);
}