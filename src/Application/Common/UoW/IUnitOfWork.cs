using Infrastructure.Interfaces;

namespace Application.Common.UoW;

public interface IUnitOfWork
{
    // Repositories
    IProductRepository ProductRepository { get; }
    ITankRepository TankRepository { get; }
    IFishRepository FishRepository { get; }
    IFishAwardRepository FishAwardRepository { get; }
    IBreedRepository BreedRepository  { get; }
    ICategoryRepository CategoryRepository { get; }

    // Transaction
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}