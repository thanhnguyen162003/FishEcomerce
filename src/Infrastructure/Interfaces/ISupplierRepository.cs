using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByUsernameAsync(string username);
        Task AddAsync(Supplier supplier);
    }
}
