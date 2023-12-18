using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public class QuotationPartRepository : IQuotationPartRepository
    {
        private readonly SparePartContext _context;

        public QuotationPartRepository(SparePartContext sparePartContext)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
        }

        public async Task<QuotationPart> AddQuotationPart(QuotationPart quotationPart)
        {
            var exists = await _context.QuotationParts
                .AnyAsync(c => c.PartId == quotationPart.PartId && c.QuoteNo == quotationPart.QuoteNo);

            if (exists)
            {
                throw new InvalidOperationException("QuotationPart with the same PartId and QuoteNo already exists.");
            }

            _context.Add(quotationPart);
            await _context.SaveChangesAsync();
            return quotationPart;
        }


        //public async Task<bool> CheckPartExistinQuoteListByPartId(int quotePartId, int partId)
        //{
        //    var exists = await _context.QuotationParts
        //        .Where(c => c.QuotePartId == quotePartId && c.PartId == partId)
        //        .FirstOrDefaultAsync();
        //    if (exists == null)
        //    {
        //        throw new InvalidOperationException($"PartID {partId} was not found in QuoteNo{quotePartId}");
        //    }

        //}



        public async Task<QuotationPart> GetQuotationPartById(int quotePartId)
        {
            var quotePart = await _context.QuotationParts.Where(c => c.QuotePartId == quotePartId).FirstOrDefaultAsync();
            if (quotePart == null)
                throw new InvalidOperationException("Quote part with " + quotePartId + " id was not found");

            return quotePart;
        }

        public async Task<QuotationPart> DeleteQuotationPart(QuotationPart quotationItem)
        {
            _context.Remove(quotationItem);
           await _context.SaveChangesAsync();
            return quotationItem;
        }

   


        public async Task<bool> QuotationPartExists(int quotationPartId)
        {
            return await _context.QuotationParts.AnyAsync(c => c.QuotePartId == quotationPartId);
        }

        public async Task<(IEnumerable<QuotationPart>, PaginationMetadata)> GetAllQuotationPartFromQuoteNo(int quoteNo, int pageSize, int pageNumber)
        {
            var collection = _context.QuotationParts.Where(c => c.QuoteNo == quoteNo).AsQueryable();
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection
                .Include(c => c.Part)
                .OrderBy(c => c.Part != null ? c.Part.PartName : null)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<QuotationPart> UpdateQuotationPart(QuotationPart quotationPart)
        {
            _context.Update(quotationPart);
            await _context.SaveChangesAsync();
            return quotationPart;
        }
    }
}
