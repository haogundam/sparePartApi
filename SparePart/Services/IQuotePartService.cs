using SparePart.Dto.Request;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;

namespace SparePart.Services
{
    public interface IQuotePartService
    {
        Task<(bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(int quoteNo, QuotePartAdd quotePartAdd);
        void RemoveQuotationPart(int quotePartId);

        Task<bool> CheckQuotePartExists(int quotePartId);
        Task<bool> CheckPartExists(int quotePartId);






    }
}
