namespace FishEcomerce.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    Task<T?> GetByIdAsync(Guid? id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    void UpdateRange(List<T> entities);
}
