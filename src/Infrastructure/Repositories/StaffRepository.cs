using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(KingFishDbContext context) : base(context)
        {
        }
        
        public async Task<Staff?> GetByUsernameAsync(string username)
        {
            return await Entities.FirstOrDefaultAsync(s => s.Username == username && s.DeletedAt == null);
        }

        public async Task<bool> CheckUsernameAsync(string username)
        {
            return await Entities.AnyAsync(s => s.Username == username && s.DeletedAt == null);
        }

        public async Task<Staff?> GetStaffByIdAsync(Guid staffId)
        {
            return await Entities.FirstOrDefaultAsync(s => s.Id == staffId && s.DeletedAt == null);
        }

        public async Task<bool> CheckAdminAsync(Guid staffId)
        {
            return await Entities.AnyAsync(s => s.Id == staffId && s.DeletedAt == null && s.IsAdmin == true);
        }
    }
}
