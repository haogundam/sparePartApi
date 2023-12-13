using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;
using SparePart.Services;

namespace SparePart.Controllers
{
    [Route("api/customers/{customerid}/quotations/{quoteNo}")]
    [ApiController]
    public class QuotePartController : ControllerBase
    {
        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;
        private readonly IQuotePartService _quotePartService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IQuotationPartRepository _quotationPartRepository;
        private readonly IMapper _mapper;
        const int PageSize = 10;

        public QuotePartController(IQuotationService quotationService,ICustomerService customerService, ICustomerRepository customerRepository, IQuotationRepository quotationRepository,
            IQuotationPartRepository quotationPartRepository,IQuotePartService quotePartService,IMapper mapper)
        {
            _quotationService = quotationService;
            _customerService = customerService;

            _customerRepository = customerRepository;
            _quotationRepository = quotationRepository;
            _quotationPartRepository = quotationPartRepository;
            _quotePartService = quotePartService ?? throw new ArgumentNullException(nameof(quotePartService));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<QuotationPartResponse>> GetAllPartsInQuoationListById(int customerId, int quoteNo, int pageNumber)
        {
            if (await _customerService.CheckCustomerExist(customerId)== false)
            {
                return NotFound();
            }

            var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

            if (quoteListByQuoteNo == null)
            {
                return NotFound();
            }

            var (quotationPart, partPaginationMetadata) = await _quotationService.GetCustomerQuotationPartFromQuoteNo(customerId, quoteNo,PageSize, pageNumber);

            Response.Headers.Add("X-Pagination",
                     System.Text.Json.JsonSerializer.Serialize(partPaginationMetadata));

            return Ok(quotationPart);

        }




        [HttpPost]
        public async Task<ActionResult> AddQuotationPart([FromBody]QuotationPartDto quotationPartDto)
        {
            if (!await _quotePartService.CheckPartExists(quotationPartDto.PartId))
            {
                return BadRequest($"Part with ID {quotationPartDto.PartId} was not found.");
            }

            bool quantityCheck, priceCheck;
            var quotationPart = new QuotationPart
            {
                QuoteNo = quotationPartDto.QuoteNo,
                PartId = quotationPartDto.PartId,
                Quantity = quotationPartDto.Quantity,
                UnitPrice = quotationPartDto.UnitPrice
            };

            (quantityCheck, priceCheck) = await _quotePartService.AddQuotationPartAsync(quotationPart);

            if (!quantityCheck && !priceCheck)
            {
                return BadRequest("Quantity exceeds available stock and the selling price is lower than the base price.");
            }
            else if (!quantityCheck)
            {
                return BadRequest("Quantity exceeds available stock.");
            }
            else if (!priceCheck)
            {
                return BadRequest("The selling price is lower than the base price.");
            }

            return Ok(quotationPartDto);
        }

        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveQuotationPart(int quotePartId) {
            if (!await _quotePartService.CheckQuotePartExists(quotePartId))
            {
                return BadRequest("Part with " + quotePartId + " id was not found.");
            }
            _quotePartService.RemoveQuotationPart(quotePartId);
            return Ok();
        }

        [HttpPatch("submit")]
        public async Task<ActionResult> SubmitQuotationList(int customerId, int quoteNo)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound();
            }

            var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

            if (quoteListByQuoteNo == null)
            {
                return NotFound();
            }

            await _quotationService.SubmitQuotationList(quoteListByQuoteNo);

            return Ok();
        }

    }
}
