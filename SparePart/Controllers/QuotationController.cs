using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.Services;

namespace SparePart.Controllers
{
    [Route("api/customers/{customerid}/quotations")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;

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
                return NotFound();
            }

            var (quotationList, paginationMetadata) = await _quotationService.GetQuoteListByCustomerIdAndPageData(customerId, pageNumber);

            if (quotationList == null)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(quotationList);

        }


        [HttpPost]
        public async Task<ActionResult> CreateNewQuotationList(int customerId)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound();
            }

            await _quotationService.CreateQuoteListByCustomerId(customerId);

            return Ok();

        }

    }
}
