using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparePart.ModelAndPersistance;

namespace SparePart.ModelAndPersistance.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SparePartContext _context;

        public CustomerRepository(SparePartContext sparePartContext)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            // Check if a Customer with the same Email or Contact No. already exists.
            var exists = await _context.Customers
                .AnyAsync(c => c.CustomerEmail == customer.CustomerEmail || c.CustomerContact == customer.CustomerContact);

            if (exists)
            {
                throw new InvalidOperationException("Customer with the same Customer info already exists.");
            }

            // Add the new customer to the context
            _context.Add(customer);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public Task<Customer> DeleteCustomer(Customer customer)
        {
            _context.Remove(customer);
            _context.SaveChangesAsync();
            return Task.FromResult(customer);
        }

        public async Task<(IEnumerable<Customer>, PaginationMetadata)> GetAllCustomers(int pageSize, int pageNumber)
        {
            var collection = _context.Customers.AsQueryable();

            var totalCustomerCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalCustomerCount, pageSize, pageNumber);

            var ListToReturn = await collection
                .OrderBy(c => c.CustomerId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (ListToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<Customer>, PaginationMetadata)> SearchCustomerById(string? searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Where(a => a.CustomerId.ToString().Contains(searchQuery));

            }

            var totalCustomerCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalCustomerCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.CustomerId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<Customer?> GetCustomerByCustomerId(int customerId)
        {
            
            return await _context.Customers
                .Where(c => c.CustomerId == customerId).FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetCustomerByQuoteNo(int quoteNo)
        {
            return await _context.Customers
                .Join(_context.QuotationLists,
                     customer => customer.CustomerId,
                     quotation => quotation.CustomerId,
                     (customer, quotation) => new { Customer = customer, Quotation = quotation })
                 .Where(cq => cq.Quotation.QuoteNo == quoteNo)
                    .Select(cq => cq.Customer)
                    .FirstOrDefaultAsync();
        }



        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            var customer = await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
            if (customer == false)
            {
                return false;
            }
            return true;
        }

        public async Task<(IEnumerable<Customer>, PaginationMetadata)> SearchCustomerByCustomerName(string? searchQuery, int pageSize, int pageNumber)
        {

            var collection = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Where(a => a.CustomerName.StartsWith(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.CustomerName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

    }
}
