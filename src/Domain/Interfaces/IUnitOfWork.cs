namespace FishEcomerce.Domain.Interfaces;

public interface IUnitOfWork
{
    // Repositories
    IProductRepository ProductRepository { get; }
    
    
    
    
    
    // Transaction
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
