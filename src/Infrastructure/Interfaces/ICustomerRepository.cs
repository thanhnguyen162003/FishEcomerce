using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task AddAsync(Customer customer);

        Task<IEnumerable<Customer>> GetAllAsync(); // Lấy tất cả khách hàng
        Task<Customer?> GetByIdAsync(Guid id); // Lấy khách hàng theo ID    
        Task UpdateAsync(Customer customer); // Cập nhật thông tin khách hàng
        Task DeleteAsync(Guid id); // Xóa khách hàng

    }
}
