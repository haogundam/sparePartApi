using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;
using SparePart.Profile;
using SparePart.Services;
using System.Collections.Generic;

namespace SparePart.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        const int PageSize = 10;
        public CustomerController(ICustomerService customerService, ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<(IEnumerable<CustomersInfo>,int)>> GetAllCustomers([FromQuery] string? name, int pageNumber)
        {
            var (customerEntities, paginationMetadata) = await _customerRepository.SearchCustomerByCustomerName(name, PageSize, pageNumber);

            if (customerEntities == null)
            {
                return NotFound();
            }
            
            _mapper.Map<IEnumerable<CustomersInfo>>(customerEntities);

            Response.Headers.Add("X-Pagination",
                  System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok((_mapper.Map<IEnumerable<CustomersInfo>>(customerEntities), paginationMetadata.TotalPageCount));

        }

        //[HttpGet("{customerid}")]
        //public async Task<ActionResult<QuotationResponse>> GetCustomer(int customerId)
        //{
        //    var customer = await _customerRepository.GetCustomerByCustomerId(customerId);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(_mapper.Map<QuotationResponse>(customer));
        //}




        [HttpPost]
        public async Task<ActionResult<CustomerDto>> RegisterCustomer([FromBody] RegisterCustomerRequest request)
        {
            if (request.CustomerName == null || request.CustomerContact == null || request.CustomerEmail == null || request.CustomerAddress == null)
            {
                return BadRequest("One of the field is EMPTY !!");
            }

            await _customerService.RegisterNewCustomer(request);

            return Ok("New Customer Registered!!");
        }






    }
}
