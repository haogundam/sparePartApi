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
        const int PageSize = 10;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersInfo>>> SearchCustomers([FromQuery] string? name, int pageNumber = 1)
        {
            var (customerEntities, paginationMetadata) = await _customerService.SearchCustomerByName(name, PageSize, pageNumber);

            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1 )
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (customerEntities, paginationMetadata) = await _customerService.SearchCustomerByName(name, PageSize, pageNumber);
            }

            if (customerEntities == null)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination",
                  System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(customerEntities);

        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> RegisterCustomer([FromBody] RegisterCustomerRequest request)
        {
            var validation =  await _customerService.ValidateCustomerRegistration(request);
            if (validation == false)
            {
                return BadRequest("One of the field is empty");
            }

            await _customerService.RegisterNewCustomer(request);

            return Ok("New Customer Registered!!");
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




    }
}
