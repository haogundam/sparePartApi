using Microsoft.AspNetCore.Mvc;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Services
{
    public class QuotePartService : IQuotePartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IQuotationPartRepository _quotationPartRepository;

        public QuotePartService(IPartRepository partRepository, IQuotationPartRepository quotationPartRepository) 
        {
            _partRepository = partRepository ?? throw new ArgumentNullException(nameof(partRepository));
            _quotationPartRepository = quotationPartRepository ?? throw new ArgumentNullException(nameof(quotationPartRepository));
        }
        public async Task<(bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(QuotationPart quotationPart)
        {
            var part = await _partRepository.GetPartById(quotationPart.PartId);

            // Check if the quantity is valid
            if (quotationPart.Quantity > await _partRepository.GetPartQuantity(part.PartId))
            {
                return (isQuantityValid: false, isPriceValid: true);
            }

            // Check if the unit price is valid
            if (quotationPart.UnitPrice > part.SellingPrice)
            {
                return (isQuantityValid: true, isPriceValid: false);
            }

            // Both quantity and price are valid, add the QuotationPart
            await _quotationPartRepository.AddQuotationPart(quotationPart);
            return (isQuantityValid: true, isPriceValid: true);
        }

        public async void RemoveQuotationPart(int quotePartId)
        {
            var quotationPart = await _quotationPartRepository.GetQuotationPartById(quotePartId);
            await _quotationPartRepository.DeleteQuotationPart(quotationPart);
        }

        public async Task<bool> CheckQuotePartExists(int quotePartId)
        {
            var exists = await _quotationPartRepository.GetQuotationPartById(quotePartId);
            if (exists == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckPartExists(int partId)
        {
            var exists = await _partRepository.GetPartById(partId);
            if (exists == null)
            {
                return false;
            }
            return true;
        }







    }
}
