using Application.CustomerFeature.Models;
using Domain.Entites;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.CustomerFeature.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Lấy tất cả khách hàng
        public async Task<IEnumerable<CustomerCreateModel>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            // Chuyển đổi từ thực thể Customer sang DTO
            return customers.Select(c => new CustomerCreateModel
            {
                Id = c.Id, // Thêm ID vào đây
                Email = c.Email,
                Name = c.Name,
                Phone = c.Phone,
                Address = c.Address,
                Birthday = c.Birthday
            });
        }


        // Thêm khách hàng mới
        public async Task AddCustomerAsync(CustomerCreateModel customerModel)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = customerModel.Email,
                Name = customerModel.Name,
                Phone = customerModel.Phone,
                Address = customerModel.Address,
                Birthday = customerModel.Birthday,
                CreatedAt = DateTime.Now
            };

            await _customerRepository.AddAsync(customer);
        }

        // Cập nhật thông tin khách hàng
        public async Task UpdateCustomerAsync(Guid id, CustomerUpdateModel customerModel)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer != null)
            {
                customer.Email = customerModel.Email;
                customer.Name = customerModel.Name;
                customer.Phone = customerModel.Phone;
                customer.Address = customerModel.Address;
                customer.Birthday = customerModel.Birthday;
                customer.UpdatedAt = DateTime.Now;

                await _customerRepository.UpdateAsync(customer);
            }
        }

        // Xóa khách hàng theo ID

        public async Task DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);
        }
    }
}
