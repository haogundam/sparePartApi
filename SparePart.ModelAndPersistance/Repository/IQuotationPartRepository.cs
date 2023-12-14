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
    public interface IQuotationPartRepository
    {

        
        Task<bool> QuotationPartExists(int quotationPartId);

        Task<QuotationPart> GetQuotationPartById(int quotePartId);

        Task<QuotationPart> AddQuotationPart(QuotationPart quotationPart);

        Task<QuotationPart> DeleteQuotationPart(QuotationPart quotationItem);

        Task<QuotationPart> UpdateQuotationPart(QuotationPart quotationPart);

        Task<(IEnumerable<QuotationPart>, PaginationMetadata)> GetAllQuotationPartFromQuoteNo(int quoteNo, int pageSize, int pageNumber);

    }
}
