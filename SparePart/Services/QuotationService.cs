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
        const int PageSize = 10;

        public QuotationService(IQuotationPartRepository quotationPartRepository,ICustomerRepository customerRepository, IQuotationRepository quotationRepository, IMapper mapper)
        {
            _quotationPartRepository = quotationPartRepository;
            _customerRepository = customerRepository;
            _quotationRepository = quotationRepository;
            _mapper = mapper;
        }

        public async Task<QuotationList> GetCustomerQuoteListByQuoteNo(int customerId, int quoteNo)
        {
            var quoteList = await _quotationRepository.GetQuoteListByQuoteNo(customerId, quoteNo);
            return quoteList;
        }

        public async Task<(QuotationPartResponse, PaginationMetadata)> GetCustomerQuotationPartFromQuoteNo(int customerId,int quoteNo, int pageNumber)
        {
            var quoteList = await _quotationRepository.GetQuoteListByQuoteNo(customerId, quoteNo);
            var (quotationPart, partPaginationMetadata) = await _quotationPartRepository.GetAllQuotationPartFromQuoteNo(quoteNo, PageSize, pageNumber);

            var response = new QuotationPartResponse
            {
                QuoteNo = quoteList.QuoteNo,
                QuoteDate = quoteList.QuoteDate.ToString("yyy-MM-dd"),
                TotalAmount = quoteList.TotalAmount,
                Parts = _mapper.Map<ICollection<PartsInQuotationList>>(quotationPart),
                TotalPageCount = partPaginationMetadata.TotalPageCount
            };

            return (response, partPaginationMetadata);
        }

        public async Task<(QuotationListResponse, PaginationMetadata)> GetQuoteListByCustomerIdAndPageData(int customerId, int pageNumber)
        {
            var customer = await _customerRepository.GetCustomerByCustomerId(customerId);

            var (quotationList, paginationMetadata) = await _quotationRepository.GetQuoteListByCustomerId(customerId, PageSize, pageNumber);

            var response = new QuotationListResponse
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,

                QuotationList = quotationList.Select(q => new QuotationResponse
                {
                    QuoteNo = q.QuoteNo,
                    QuoteDate = q.QuoteDate.ToString("yyyy-MM-dd"),
                    QuoteValidDate = q.QuoteValidDate.ToString("yyyy-MM-dd"),
                    Status = q.Status
                }).ToList(),

                TotalPageCount = paginationMetadata.TotalPageCount
            };

            return (response, paginationMetadata);

        }

        public async Task CreateQuoteListByCustomerId(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByCustomerId(customerId);

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
        }

        public async Task SubmitQuotationList(QuotationList quotationList)
        {
            quotationList.Status = Status.paid;

            await _quotationRepository.UpdateQuotationList(quotationList);
        }

    }
}
