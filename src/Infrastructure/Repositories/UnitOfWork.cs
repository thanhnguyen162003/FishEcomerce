using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.Extensions.DependencyInjection.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly KingFishContext _context;
    private IDbContextTransaction? _transaction;


    // Interface Repo
    private IProductRepository _productRepository = null!;

    public UnitOfWork(KingFishContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null)
        {
            return;
        }

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("A transaction has not been started.");
        }

        try
        {
            await _transaction.CommitAsync();
            _transaction.Dispose();
            _transaction = null;
        }
        catch (Exception)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }

            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
        
    }
}
