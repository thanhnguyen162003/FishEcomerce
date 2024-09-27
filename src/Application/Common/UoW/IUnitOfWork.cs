using Infrastructure.Interfaces;

namespace Application.Common.UoW;

public interface IUnitOfWork
{
    IRepository Repository { get; }
}