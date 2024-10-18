using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff?> GetByUsernameAsync(string username);
        Task<bool> CheckUsernameAsync(string username);
        Task<Staff?> GetStaffByIdAsync(Guid staffId);
        Task<bool> CheckAdminAsync(Guid staffId);
    }
}
