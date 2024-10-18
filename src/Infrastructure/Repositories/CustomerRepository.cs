using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(KingFishDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByUsernameAsync(string username)
        {
            return await Entities.FirstOrDefaultAsync(c => c.Username == username && c.DeletedAt == null);
        }

        public async Task<bool> CheckUserByUsernameRegister(string username)
        {
            return await Entities.AnyAsync(x => (x.Username == username && x.DeletedAt == null));
        }

        public async Task<Customer?> GetCustomerById(Guid customerId)
        {
            return await Entities.FirstOrDefaultAsync(x => x.Id == customerId && x.DeletedAt == null);
        }

        public async Task<string?> GetCustomerName(Guid customerId)
        {
            return await Entities.Where(x => x.Id == customerId).Select(x => x.Name).FirstOrDefaultAsync();
        }
    }
}
