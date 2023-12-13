using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePart.ModelAndPersistance.Entities;

namespace SparePart.ModelAndPersistance.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task<Customer> DeleteCustomer(Customer customer);
        Task<(IEnumerable<Customer>, PaginationMetadata)> GetAllCustomers(int pageSize, int pageNumber);
        Task<(IEnumerable<Customer>, PaginationMetadata)> SearchCustomerById(string? searchQuery, int pageSize, int pageNumber);
        Task<Customer> GetCustomerByCustomerId(int customerId);

        Task<bool> CustomerExistsAsync(int customerId);
        Task<(IEnumerable<Customer>, PaginationMetadata)> SearchCustomerByCustomerName(string? searchQuery, int pageSize, int pageNumber);
    }
}
