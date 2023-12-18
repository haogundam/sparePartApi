using SparePart.Dto.Request;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;

namespace SparePart.Services
{
    public interface IQuotePartService
    {
          Task<(bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(int quoteNo, QuotePartAdd quotePartAdd);
        //Task<(bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(int quoteNo, int partId, int quantity, double unitPrice);

        Task<QuotationPart> CheckQuotePartExistsinSpecificQuoteList(int quotePartId);
       // void RemoveQuotationPart(int quotePartId);

        Task<bool> CheckQuotePartExists(int quotePartId);
        Task<bool> CheckPartExists(int quotePartId);






    }
}
