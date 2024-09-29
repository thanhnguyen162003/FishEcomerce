using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly KingFishDbContext _context;
    protected readonly DbSet<T> Entities;

    public Repository(KingFishDbContext context)
    {
        _context = context;
        Entities = context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return Entities;
    }

    public async Task<T?> GetByIdAsync(Guid? id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Entities.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        await Entities.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        Entities.Update(entity);
    }

    public void UpdateRange(List<T> entities)
    {
        Entities.UpdateRange(entities);
    }
}