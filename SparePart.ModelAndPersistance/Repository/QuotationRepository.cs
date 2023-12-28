using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly SparePartContext _context;

        public QuotationRepository(SparePartContext sparePartContext)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
        }

        public async Task<QuotationList> CreateQuotationList(QuotationList quotationList)
        {
            var exists = await QuotationListExists(quotationList.QuoteNo);
            if (exists)
            {
                throw new InvalidOperationException("QuotationList with the same QuoteNo already exists.");
            }
            _context.Add(quotationList);
            await _context.SaveChangesAsync();
            return quotationList;
        }

        public Task<QuotationList> DeleteQuotationList(QuotationList quotationList)
        {
            _context.Remove(quotationList);
            _context.SaveChangesAsync();
            return Task.FromResult(quotationList);
        }

        public async Task<bool> QuotationListExists(int quoteNo)
        {
            return await _context.QuotationLists.Where(c => c.QuoteNo == quoteNo).AnyAsync();
        }

        public async Task<QuotationList> GetQuoteListByQuoteNo(int customerId, int quoteNo)
        {
            var quotationList = await _context.QuotationLists
             .Where(p => p.CustomerId == customerId && p.QuoteNo == quoteNo)
             .FirstOrDefaultAsync();

            return quotationList;
        }


        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuotationListByCustomerId(int customerId, int pageSize, int pageNumber)
        {
            var collection = _context.QuotationLists.Where(c => c.CustomerId == customerId).AsQueryable();
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var ListToReturn = await collection
                .OrderByDescending(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (ListToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuotationListByDate(int cutomerId, DateOnly quoteDate, int pageSize, int pageNumber)
        {
            var collection = _context.QuotationLists.Where(c => c.QuoteDate == quoteDate).AsQueryable();
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var ListToReturn = await collection
                .OrderByDescending(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (ListToReturn, paginationMetadata);
        }

        public async Task<QuotationList> UpdateQuotationList(QuotationList quotationList)
        {
            _context.Update(quotationList);
            await _context.SaveChangesAsync();
            return quotationList;
        }

        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetAllQuotationList(int pageSize, int pageNumber)
        {
            var collection = _context.QuotationLists.AsQueryable();
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var ListToReturn = await collection
                .OrderByDescending(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (ListToReturn, paginationMetadata);
        }


        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetAllQuotationListByStatus(int pageSize, int pageNumber, Status status)
        {
            var collection = _context.QuotationLists.Where(c => c.Status == status).AsQueryable();
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var ListToReturn = await collection
                .OrderByDescending(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (ListToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuoteListByCustomerId(int customerId, int pageSize, int pageNumber)
        {
            var collection = _context.QuotationLists.AsQueryable();


            collection = collection
                .Include(a => a.Customer)
                .Where(a => a.Customer.CustomerId == customerId);


            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }


        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuoteListByCustomerIdAndStatus(int customerId, int pageSize, int pageNumber, Status status)
        {
            var exists = await _context.QuotationLists
                //.Include(a => a.Customer)
                .Where(a => a.Customer.CustomerId == customerId && a.Status == status)
                .AnyAsync();

            if (exists == false) { return (null, null); }


            var collection = _context.QuotationLists.AsQueryable();
                        
            collection = collection
                .Include(a => a.Customer)
                .Where(a => a.Customer.CustomerId == customerId && a.Status == status);


            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<QuotationList>, PaginationMetadata)> SearchQuoteListByCustomerNameAndStatus(string? searchQuery, int pageSize, int pageNumber,Status status)
        {
            var collection = _context.QuotationLists.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Where(a => a.Status == status && a.Customer.CustomerName.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.QuoteNo)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }


    }
}
