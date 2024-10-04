using Domain.Entites;

namespace Infrastructure.Interfaces;

public interface IBlogRepository
{
    Task<Blog> GetBlogById(Guid id, CancellationToken cancellationToken);
    Task<Blog> GetBlogBySlug(string slug, CancellationToken cancellationToken);
    Task<bool> CreateBlog(Blog blog, CancellationToken cancellationToken);
    Task<bool> UpdateBlog(Blog blog, CancellationToken cancellationToken);
    Task<bool> DeleteBlog(Guid id, CancellationToken cancellationToken);
}