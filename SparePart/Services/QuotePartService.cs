using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparePart.Dto.Request;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Services
{
    public class QuotePartService : IQuotePartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IQuotationPartRepository _quotationPartRepository;
        private readonly IMapper _mapper;

        public QuotePartService(IPartRepository partRepository, IQuotationPartRepository quotationPartRepository,IMapper mapper) 
        {
            _partRepository = partRepository ?? throw new ArgumentNullException(nameof(partRepository));
            _quotationPartRepository = quotationPartRepository ?? throw new ArgumentNullException(nameof(quotationPartRepository));
            _mapper = mapper;
        }



        public async Task<(QuotationPart,bool isQuantityValid, bool isPriceValid)> AddQuotationPartAsync(int quoteNo, QuotePartAdd quotePartAdd)
        {
            var part = await _partRepository.GetPartById(quotePartAdd.PartId);

            // Check if the quantity is valid
            if (quotePartAdd.Quantity > await _partRepository.GetPartQuantity(part.PartId,quotePartAdd.WarehouseName))
            {
                return (null,isQuantityValid: false, isPriceValid: true);
            }

            // Check if the unit price is valid
            if (quotePartAdd.UnitPrice < part.BuyingPrice)
            {
                return (null,isQuantityValid: true, isPriceValid: false);
            }

            // Both quantity and price are valid, add the QuotationPart
            var quotePart = new QuotationPart
            { 
                QuoteNo = quoteNo,
                PartId= quotePartAdd.PartId,
                Quantity = quotePartAdd.Quantity,
                UnitPrice = quotePartAdd.UnitPrice,
            };

           var newQuotePart =  await _quotationPartRepository.AddQuotationPart(quotePart);
            return (newQuotePart,isQuantityValid: true, isPriceValid: true);
        }

        public async Task<(bool isQuantityValid, bool isPriceValid)> UpdateQuotationPartAsync(int quoteNo, QuotationPart quotationPart, QuotePartUpdatePriceQuantity quotePartUpdatePriceQuantity)
        {
            var part = await _partRepository.GetPartById(quotationPart.PartId);
            if (part == null)
            {
                return (false, false);
            }

            ////Check if the quantity is valid
            if (quotePartUpdatePriceQuantity.Quantity > await _partRepository.GetPartQuantityInAllStorages(part.PartId))
            {
                return (isQuantityValid: false, isPriceValid: true);
            }

            // Check if the unit price is valid
            if (quotePartUpdatePriceQuantity.UnitPrice < part.BuyingPrice)
            {
                return (isQuantityValid: true, isPriceValid: false);
            }

            //await _quotationPartRepository.UpdateQuotationPart(quotationPart);

            return (isQuantityValid: true, isPriceValid: true);
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

        public async Task<QuotationPart> CheckQuotePartExistsinSpecificQuoteList(int quotePartId)
        {
            var exists = await _quotationPartRepository.GetQuotationPartById(quotePartId);
            if (exists == null)
            {
                return null;
            }
            return exists;
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
