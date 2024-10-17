using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly KingFishDbContext _context;

        public CustomerRepository(KingFishDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email && c.IsDeleted == true);
        }


        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        //
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.Where(c => !c.IsDeleted == true).ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }


        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await GetByIdAsync(id);
            if (customer != null)
            {
                customer.IsDeleted = true; // Đánh dấu khách hàng đã bị xóa
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string?> GetCustomerName(Guid customerId)
        {
            return await _context.Customers.Where(x => x.Id == customerId).Select(x => x.Name).FirstOrDefaultAsync();
        }
    }
}
