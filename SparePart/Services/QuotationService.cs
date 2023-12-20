using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Request;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationPartRepository _quotationPartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IMapper _mapper;

        public QuotationService(IQuotationPartRepository quotationPartRepository,ICustomerRepository customerRepository, IQuotationRepository quotationRepository, IMapper mapper)
        {
            _quotationPartRepository = quotationPartRepository;
            _customerRepository = customerRepository;
            _quotationRepository = quotationRepository;
            _mapper = mapper;
        }

        public async Task<QuotationList?> GetCustomerQuoteListByQuoteNo(int customerId, int quoteNo)
        {
            var quoteList = await _quotationRepository.GetQuoteListByQuoteNo(customerId, quoteNo);
            if (quoteList == null) { return null; }

            return quoteList;
        }

        public async Task<(QuotationPartResponse, PaginationMetadata)> GetCustomerQuotationPartFromQuoteNo(int customerId, int quoteNo, int pageSize, int pageNumber)
        {
            var quoteList = await _quotationRepository.GetQuoteListByQuoteNo(customerId, quoteNo);
            var (quotationPart, partPaginationMetadata) = await _quotationPartRepository.GetAllQuotationPartFromQuoteNo(quoteNo, pageSize, pageNumber);

            
                var response = new QuotationPartResponse
                {
                    QuoteNo = quoteList.QuoteNo,
                    QuoteDate = quoteList.QuoteDate.ToString("yyy-MM-dd"),
                    TotalAmount = quoteList.TotalAmount,
                    Parts = _mapper.Map<ICollection<PartsInQuotationList>>(quotationPart),
                };

                return (response, partPaginationMetadata);
            
        }

        public async Task<(QuotationListResponse, PaginationMetadata, PaginationMetadata)> GetQuoteListByCustomerIdAndPageData(int customerId,int pageSize, int pendingPageNumber, int paidPageNumber)
        {
            var customer = await _customerRepository.GetCustomerByCustomerId(customerId);
            if (customer == null)
            {
                return (null,null,null);
            }

            var (pendingQuotationList, pendingPaginationMetadata) = await _quotationRepository.GetQuoteListByCustomerIdAndStatus(customerId, pageSize, pendingPageNumber, Status.pending);           
            var (paidQuotationList, paidPaginationMetadata) = await _quotationRepository.GetQuoteListByCustomerIdAndStatus(customerId, pageSize, paidPageNumber, Status.paid);



            var response = new QuotationListResponse
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,

                PendingQuotationList = pendingQuotationList?.Select(q => new QuotationResponse
                {
                    QuoteNo = q.QuoteNo,
                    QuoteDate = q.QuoteDate.ToString("yyyy-MM-dd"),
                    QuoteValidDate = q.QuoteValidDate.ToString("yyyy-MM-dd"),
                }).ToList(),

                PaidQuotationList = paidQuotationList?.Select(q => new QuotationResponse
                {
                    QuoteNo = q.QuoteNo,
                    QuoteDate = q.QuoteDate.ToString("yyyy-MM-dd"),
                    QuoteValidDate = q.QuoteValidDate.ToString("yyyy-MM-dd"),
                }).ToList(),
            };

            return (response, pendingPaginationMetadata, paidPaginationMetadata);



        }

        public async Task<QuotationList?> CreateQuoteListByCustomerId(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByCustomerId(customerId);
            if (customer == null) { return null; }


            var newQuoteLists = new QuotationListForCreation
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,

                QuoteDate = DateOnly.FromDateTime(DateTime.Now),
                QuoteValidDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),

                TotalAmount = 0,
                PaymentType = "credit"

            };

            var newQuoteList = _mapper.Map<QuotationList>(newQuoteLists);

            await _quotationRepository.CreateQuotationList(newQuoteList);

            return newQuoteList;

        }

        public async Task SubmitQuotationList(QuotationList quotationList)
        {
            quotationList.Status = Status.paid;

            await _quotationRepository.UpdateQuotationList(quotationList);
        }

        public async Task UpdateTotalAmount(QuotationList quotationList)
        {
            double totalAmount = 0;
                
            foreach (var part in quotationList.QuotationParts)
            {
                double amount = part.UnitPrice * part.Quantity;
                totalAmount += amount;
            }
            
            quotationList.TotalAmount = totalAmount;
            await _quotationRepository.UpdateQuotationList(quotationList);
        
        }



    }
}
