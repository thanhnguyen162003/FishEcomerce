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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly KingFishDbContext _context;

        public SupplierRepository(KingFishDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier?> GetByUsernameAsync(string username)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Username == username);
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
