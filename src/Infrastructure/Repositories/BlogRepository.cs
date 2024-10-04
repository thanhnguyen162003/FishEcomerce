using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BlogRepository : Repository<Blog>, IBlogRepository
{
    private readonly KingFishDbContext _context;
    public BlogRepository(KingFishDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CreateBlog(Blog blog, CancellationToken cancellationToken)
    {
        await Entities.AddAsync(blog, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteBlog(Guid id, CancellationToken cancellationToken)
    {
        var blog = await GetBlogById(id, cancellationToken);
        if (blog == null)
        {
            return false;
        }
        blog.DeletedAt = DateTime.Now;
        Entities.Update(blog);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Blog> GetBlogById(Guid id, CancellationToken cancellationToken)
    {
        return await Entities.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken)!;
    }

    public async Task<Blog> GetBlogBySlug(string slug, CancellationToken cancellationToken)
    {
        return await Entities.Where(x => x.Slug.Equals(slug)).FirstOrDefaultAsync(cancellationToken)!;
    }

    public async Task<bool> UpdateBlog(Blog blog, CancellationToken cancellationToken)
    {
        var existingBlog = await Entities.FindAsync(new object[] { blog.Id }, cancellationToken);
        if (existingBlog == null) return false;

        _context.Entry(existingBlog).CurrentValues.SetValues(blog);
        foreach (var property in _context.Entry(existingBlog).Properties)
        {
            if (property.CurrentValue == null)
                property.IsModified = false;
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}