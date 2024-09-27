using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Application.Common.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly KingFishDbContext _context;
    private readonly IRepository _repository;

    public UnitOfWork(KingFishDbContext context)
    {
        _context = context;
        
    }
    public IRepository Repository { get; }
    
}