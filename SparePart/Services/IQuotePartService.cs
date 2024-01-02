using SparePart.Dto.Request;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;

namespace SparePart.Services
{
    public interface IQuotePartService
    {
        Task<(bool isQuantityValid, bool isPriceValid)> UpdateQuotationPartAsync(int quoteNo, QuotationPart quotationPart, QuotePartUpdatePriceQuantity quotePartUpdatePriceQuantity);

        Task<(QuotationPart, bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(int quoteNo, QuotePartAdd quotePartAdd);

        Task<QuotationPart> CheckQuotePartExistsinSpecificQuoteList(int quotePartId);

        Task<bool> CheckQuotePartExists(int quotePartId);

        Task<bool> CheckPartExists(int quotePartId);

    }
}
