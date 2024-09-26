using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly KingFishContext _context;
    private readonly DbSet<T> _entities;

    protected Repository(KingFishContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return _entities;
    }

    public async Task<T?> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        return await _entities.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _entities.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken)
    {
        await _entities.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        _entities.Update(entity);
    }

    public void UpdateRange(List<T> entities)
    {
        _entities.UpdateRange(entities);
    }
}
