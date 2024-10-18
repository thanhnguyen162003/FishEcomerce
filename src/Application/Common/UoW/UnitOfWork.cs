using Infrastructure.Context;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Common.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly KingFishDbContext _context;
    private IDbContextTransaction _transaction;

    // repo
    private readonly IProductRepository _productRepository;
    private readonly ITankRepository _tankRepository;
    private readonly IFishRepository _fishRepository;
    private readonly IFishAwardRepository _fishAwardRepository;
    private readonly IBreedRepository _breedRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IBlogRepository _blogRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    public UnitOfWork(KingFishDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_context);
    public ITankRepository TankRepository => _tankRepository ?? new TankRepository(_context);
    public IFishRepository FishRepository => _fishRepository ?? new FishRepository(_context);
    public IFishAwardRepository FishAwardRepository => _fishAwardRepository ?? new FishAwardRepository(_context);
    public IBreedRepository BreedRepository => _breedRepository ?? new BreedRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);
    public IFeedbackRepository FeedbackRepository => _feedbackRepository ?? new FeedbackRepository(_context);
    public ICustomerRepository CustomerRepository => _customerRepository ?? new CustomerRepository(_context);
    public IStaffRepository StaffRepository => _staffRepository ?? new StaffRepository(_context);
    public IImageRepository ImageRepository => _imageRepository ?? new ImageRepository(_context);
    public IBlogRepository BlogRepository => _blogRepository ?? new BlogRepository(_context);
    public IOrderRepository OrderRepository => _orderRepository ?? new OrderRepository(_context);
    public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository ?? new OrderDetailRepository(_context);


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