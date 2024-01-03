using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Repository;
using SparePart.Services;

namespace SparePart.Controllers
{
    [Route("api/quotations")]
    //[Authorize]
    [ApiController]
    public class QuotationListByQuoteNoController : ControllerBase
    {

        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IQuotePartService _quotePartService;
        private readonly IPartRepository _partRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IQuotationPartRepository _quotationPartRepository;

        const int PageSize = 10;

        public QuotationListByQuoteNoController(IQuotationService quotationService
            ,IQuotationRepository quotationRepository)
        {
            _quotationService = quotationService;
            _quotationRepository = quotationRepository;
        }

        [HttpGet("{quoteNo}")]
        public async Task<ActionResult<QuotationPartResponseByQuoteNo>> GetQuotePartByQuoteNo(int quoteNo,int pageNumber)
        {
            if (await _quotationRepository.QuotationListExists(quoteNo) == false)
            {
                return BadRequest($"No this QuoteNo {quoteNo}");
            }

            var (quotePart,paginationMetadata) = await _quotationService.GetQuoteListByQuoteNo(quoteNo,PageSize,pageNumber);

            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1)
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (quotePart, paginationMetadata) = await _quotationService.GetQuoteListByQuoteNo(quoteNo, PageSize, pageNumber);
            }
            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(quotePart);

        }


    }
}
