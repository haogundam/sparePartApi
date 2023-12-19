using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
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
        public async Task<ActionResult<QuotationListResponse>> GetQuotationListsByCustomerId(int customerId, int pageNumber)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound("No this customer");
            }

            var (quotationList, paginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, PageSize,pageNumber);

            if (quotationList == null)
            {
                return NotFound("");
            }

            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1)
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (quotationList, paginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, PageSize, pageNumber);
            }

            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(quotationList);

        }


        [HttpPost]
        public async Task<ActionResult> CreateNewQuotationList(int customerId)
        {

            if (await _quotationService.CreateQuoteListByCustomerId(customerId) == null)
            {
                return NotFound("No this customer Id");
            }

            return Ok("New QuoteList Created!");

        }

    }
}
