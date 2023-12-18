using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Request;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;
using SparePart.Services;
using System.Xml.Linq;

namespace SparePart.Controllers
{
    [Route("api/customers/{customerId}/quotations/{quoteNo}")]
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
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound("No this customer id");
            }

            var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

            if (quoteListByQuoteNo == null)
            {
                return NotFound($"Customer ID {customerId} dont have this QuoteNo {quoteNo}");
            }

            var (quotationPart, paginationMetadata) = await _quotationService.GetCustomerQuotationPartFromQuoteNo(customerId, quoteNo,PageSize, pageNumber);

            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1)
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (quotationPart, paginationMetadata) = await _quotationService.GetCustomerQuotationPartFromQuoteNo(customerId, quoteNo, PageSize, pageNumber);
            }

            double totalAmount = 0;

            foreach (var part in quoteListByQuoteNo.QuotationParts)
            {
                double amount = part.UnitPrice * part.Quantity;
                totalAmount += amount;
            }

            quoteListByQuoteNo.TotalAmount = totalAmount;
            await _quotationRepository.UpdateQuotationList(quoteListByQuoteNo);


            Response.Headers.Add("X-Pagination",
                     System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(quotationPart);

        }

        [HttpPost]
        public async Task<ActionResult> AddNewQuotePart( int customerId, int quoteNo, [FromBody] QuotePartAdd quotePartAdd)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound("No this customer id");
            }

            var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

            if (quoteListByQuoteNo == null)
            {
                return NotFound($"Customer ID {customerId} dont have this QuoteNo {quoteNo}");
            }

            var (quantityCheck, priceCheck) = await _quotePartService.AddQuotationPartAsync(quoteNo, quotePartAdd);

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

            // edit total amount when add 
            double totalAmount = 0;

            foreach (var part in quoteListByQuoteNo.QuotationParts)
            {
                double amount = part.UnitPrice * part.Quantity;
                totalAmount += amount;
            }

            quoteListByQuoteNo.TotalAmount = totalAmount;
            await _quotationRepository.UpdateQuotationList(quoteListByQuoteNo);



            //await _quotationService.UpdateTotalAmount(quoteListByQuoteNo);

            return Ok($"PartID {quotePartAdd.PartId} added to QuoteNo {quoteNo}");

        }

        [HttpDelete("quoteparts/{quotePartId}")]
        public async Task<ActionResult> RemoveQuotePart( int customerId, int quoteNo, int quotePartId)
        {
            if (await _customerService.CheckCustomerExist(customerId) == false)
            {
                return NotFound($"No this customer id");
            }

            var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

            if (quoteListByQuoteNo == null)
            {
                return NotFound($"Customer ID {customerId} dont have this QuoteNo {quoteNo}");
            }

            if (await _quotePartService.CheckQuotePartExists(quotePartId) == false)
            {
                return NotFound($"No this QuotePartId{quotePartId}");
            }

            var exists = await _quotePartService.CheckQuotePartExistsinSpecificQuoteList(quotePartId);
            if (exists.QuoteNo != quoteNo)
            {
                return NotFound($"This QuotePart {quotePartId} is not in QuoteList {quoteNo}");
            }

            var quotationPart = await _quotationPartRepository.GetQuotationPartById(quotePartId);
            await _quotationPartRepository.DeleteQuotationPart(quotationPart);


            double totalAmount = 0;

            foreach (var part in quoteListByQuoteNo.QuotationParts)
            {
                double amount = part.UnitPrice * part.Quantity;
                totalAmount += amount;
            }

            quoteListByQuoteNo.TotalAmount = totalAmount;
            await _quotationRepository.UpdateQuotationList(quoteListByQuoteNo);




            //_quotePartService.RemoveQuotationPart(quotePartId);
            return Ok($"Successfuly remove QuotepartID {quotePartId} from QuoteList {quoteNo}");

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

            return Ok("Submitted!");
        }


        //[HttpPost]
        //public async Task<ActionResult<QuotationPart>> AddQuotationPart([FromQuery]int customerId, int quoteNo,[FromBody]QuotePartAdd quotePartAdd)
        //{
        //    if (await _customerService.CheckCustomerExist(customerId) == false)
        //    {
        //        return NotFound();
        //    }

        //    var quoteListByQuoteNo = await _quotationService.GetCustomerQuoteListByQuoteNo(customerId, quoteNo);

        //    if (quoteListByQuoteNo == null)
        //    {
        //        return NotFound();
        //    }

        //    bool quantityCheck, priceCheck;

        //     _mapper.Map<QuotationPart>(quotePartAdd);
        //    var quotationPart = _mapper.Map<QuotationPart>(quoteNo);


        //    (quantityCheck, priceCheck) = await _quotePartService.AddQuotationPartAsync(quotationPart);

        //    if (!quantityCheck && !priceCheck)
        //    {
        //        return BadRequest("Quantity exceeds available stock and the selling price is lower than the base price.");
        //    }
        //    else if (!quantityCheck)
        //    {
        //        return BadRequest("Quantity exceeds available stock.");
        //    }
        //    else if (!priceCheck)
        //    {
        //        return BadRequest("The selling price is lower than the base price.");
        //    }

        //    return Ok(quotationPart);
        //}

        //[HttpDelete]
        //public async Task<ActionResult> RemoveQuotationPart(int quotePartId) {
        //    if (!await _quotePartService.CheckQuotePartExists(quotePartId))
        //    {
        //        return BadRequest("Part with " + quotePartId + " id was not found.");
        //    }
        //    _quotePartService.RemoveQuotationPart(quotePartId);
        //    return Ok();
        //}



    }
}
