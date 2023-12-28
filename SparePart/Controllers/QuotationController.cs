using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.Services;
using System.Xml.Linq;

namespace SparePart.Controllers
{
    [Route("api/customers/{customerid}/quotations")]
    [Authorize]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;
        const int PageSize = 10;
        public QuotationController( IQuotationService quotationService, ICustomerService customerService)
        {
            _quotationService = quotationService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<QuotationListResponse>> GetQuotationListsByCustomerId(int customerId, int pendingPageNumber = 1, int paidPageNumber = 1)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound("No this customer");
            }

            var (quotationList, pendingPaginationMetadata, paidPaginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, PageSize, pendingPageNumber, paidPageNumber);

            if (pendingPaginationMetadata != null && pendingPageNumber > pendingPaginationMetadata.TotalPageCount || pendingPageNumber < 1)
            {
                pendingPageNumber = pendingPaginationMetadata.TotalPageCount;
                (quotationList, pendingPaginationMetadata, paidPaginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, PageSize, pendingPageNumber, paidPageNumber);
            }

            if (paidPaginationMetadata != null && paidPageNumber > paidPaginationMetadata.TotalPageCount || paidPageNumber < 1)
            {
                paidPageNumber = paidPaginationMetadata.TotalPageCount;
                (quotationList, pendingPaginationMetadata, paidPaginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, PageSize, pendingPageNumber, paidPageNumber);
            }

            Response.Headers.Add("Pending-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(pendingPaginationMetadata));

            Response.Headers.Add("Paid-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paidPaginationMetadata));

            return Ok(quotationList);

        }


        [HttpPost]
        public async Task<ActionResult> CreateNewQuotationList(int customerId)
        {
            var newQuoteList = await _quotationService.CreateQuoteListByCustomerId(customerId);

            if (newQuoteList == null)
            {
                return NotFound("No this customer Id");
            }

            return Ok(newQuoteList.QuoteNo);

        }

    }
}
