using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;

namespace SparePart.Services
{
    public interface IQuotationService
    {
        Task<QuotationList> GetCustomerQuoteListByQuoteNo(int customerId, int quoteNo);

        Task<(QuotationPartResponse, PaginationMetadata)> GetCustomerQuotationPartFromQuoteNo(int customerId, int quoteNo, int pageSize, int pageNumber);

        Task<(QuotationListResponse?, PaginationMetadata)> GetQuoteListByCustomerIdAndPageData(int customerId, int pageSize, int pageNumber);

        Task<QuotationList?> CreateQuoteListByCustomerId(int customerId);

        Task SubmitQuotationList(QuotationList quotationList);

    }
}
