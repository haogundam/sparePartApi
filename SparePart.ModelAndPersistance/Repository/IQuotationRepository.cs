using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public interface IQuotationRepository
    {
        Task<QuotationList> CreateQuotationList(QuotationList quotationList);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuotationListByCustomerId(int cutomerId, int pageSize, int pageNumber);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuotationListByDate(int cutomerId, DateOnly quoteDate, int pageSize, int pageNumber);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetAllQuotationList(int pageSize, int pageNumber);

        Task<bool> QuotationListExists(int quoteNo);
        Task<QuotationList> GetQuoteListByQuoteNo(int customerId,int quoteNo);
        Task<QuotationList> UpdateQuotationList(QuotationList quotationList);

        Task<QuotationList> DeleteQuotationList(QuotationList quotationList);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetAllQuotationListByStatus(int pageSize, int pageNumber, Status status);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuoteListByCustomerId(int customerId, int pageSize, int pageNumber);

        Task<(IEnumerable<QuotationList>, PaginationMetadata)> GetQuoteListByCustomerIdAndStatus(int customerId, int pageSize, int pageNumber, Status status);
       
        Task<(IEnumerable<QuotationList>, PaginationMetadata)> SearchQuoteListByCustomerNameAndStatus(string? searchQuery, int pageSize, int pageNumber, Status status);
    }

}
