using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> GetByUsernameAsync(string username);
        Task<bool> CheckUserByUsernameRegister(string username);
        Task<Customer?> GetCustomerById(Guid customerId);
        
        // Temp
        Task<string?> GetCustomerName(Guid customerId);
    }
}
