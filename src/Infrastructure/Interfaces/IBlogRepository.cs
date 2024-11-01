using Domain.Entites;
using Domain.QueriesFilter;

namespace Infrastructure.Interfaces;

public interface IBlogRepository
{
    Task<Blog?> GetBlogById(Guid id, CancellationToken cancellationToken);
    Task<Blog> GetBlogBySlug(string slug, CancellationToken cancellationToken);
    Task<List<Blog>> GetAllAsync(BlogQueryFilter blogQueryFilter, CancellationToken cancellationToken);
    Task<bool> CreateBlog(Blog blog, CancellationToken cancellationToken);
    Task<bool> UpdateBlog(Blog blog, CancellationToken cancellationToken);
    Task<bool> DeleteBlog(Guid id, CancellationToken cancellationToken);
    Task<bool> CheckBlogExists(Guid id, CancellationToken cancellationToken);
    Task<List<Blog>> SearchBlogs(string title, CancellationToken cancellationToken);
}