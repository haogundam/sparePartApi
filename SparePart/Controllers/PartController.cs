using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Controllers
{
    [Route("api/parts")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IQuotationRepository _quotationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IQuotationPartRepository _quotationPartRepository;
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;
        const int PageSize = 10;
        public PartController(IQuotationRepository quotationRepository, ICustomerRepository customerRepository,
            IQuotationPartRepository quotationPartRepository, IPartRepository partRepository, IMapper mapper)
        {
            _quotationRepository = quotationRepository;
            _customerRepository = customerRepository;
            _quotationPartRepository = quotationPartRepository;
            _partRepository = partRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartResponse>>> GetAllParts(string? sku, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.SearchPartsBySKU(sku, PageSize, pageNumber);
            if (parts == null) { return NotFound(); }

            var skuPartResponses = new List<PartResponse>();

            foreach (var part in parts)
            {
                var supplierName = await _partRepository.GetSupplierNameByPartId(part.PartId);
                var warehouseName = await _partRepository.GetWarehouseNameByPartId(part.PartId);
                var quantityLeft = await _partRepository.GetPartQuantity(part.PartId);

                var partResponse = _mapper.Map<PartResponse>(part);

                partResponse.SupplierName = supplierName;
                partResponse.WarehouseName = warehouseName;
                partResponse.Quantity = quantityLeft;

                skuPartResponses.Add(partResponse);
            }

            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));



            return Ok(skuPartResponses);

        }











        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<PartResponse>>> GetSameCategoryParts(string? sku, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.GetPartsSameCategoryBySKU(sku, PageSize, pageNumber);
            if (parts == null) { return NotFound(); }

            var skuPartResponses = new List<PartResponse>();

            foreach (var part in parts)
            {
                var supplierName = await _partRepository.GetSupplierNameByPartId(part.PartId);
                var warehouseName = await _partRepository.GetWarehouseNameByPartId(part.PartId);
                var quantityLeft = await _partRepository.GetPartQuantity(part.PartId);

                var partResponse = _mapper.Map<PartResponse>(part);

                partResponse.SupplierName = supplierName;
                partResponse.WarehouseName = warehouseName;
                partResponse.Quantity = quantityLeft;

                skuPartResponses.Add(partResponse);
            }

            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            

            return Ok(skuPartResponses);


            //var (partsBySKU, skuPaginationMetadata) = await _partRepository.SearchPartsBySKU(sku, PageSize, pageNumber);

            //if (partsBySKU == null)
            //{
            //    return NotFound();
            //}

            //var skuPartResponses = new List<PartResponse>();

            //foreach (var part in partsBySKU)
            //{
            //    var supplierName = await _partRepository.GetSupplierNameByPartId(part.PartId);
            //    var warehouseName = await _partRepository.GetWarehouseNameByPartId(part.PartId);
            //    var quantityLeft = await _partRepository.GetPartQuantity(part.PartId);

            //    var partResponse = _mapper.Map<PartResponse>(part);

            //    partResponse.SupplierName = supplierName;
            //    partResponse.WarehouseName = warehouseName;
            //    partResponse.Quantity = quantityLeft;

            //    skuPartResponses.Add(partResponse);
            //}

            //var (partsByCategory, categoryPaginationMetadata) = await _partRepository.SearchPartsBySKU(sku, PageSize, pageNumber);

            //var categoryPartResponses = new List<PartResponse>();

            //foreach (var part in partsByCategory)
            //{
            //    var supplierName = await _partRepository.GetSupplierNameByPartId(part.PartId);
            //    var warehouseName = await _partRepository.GetWarehouseNameByPartId(part.PartId);
            //    var quantityLeft = await _partRepository.GetPartQuantity(part.PartId);

            //    var partResponse = _mapper.Map<PartResponse>(part);

            //    partResponse.SupplierName = supplierName;
            //    partResponse.WarehouseName = warehouseName;
            //    partResponse.Quantity = quantityLeft;

            //    categoryPartResponses.Add(partResponse);
            //}

            //Response.Headers.Add("X-Pagination",
            //     System.Text.Json.JsonSerializer.Serialize(categoryPaginationMetadata));

            //var mergedPartResponses = skuPartResponses.Concat(categoryPartResponses)
            //    .GroupBy(p => p.PartId).Select(g => g.First()).ToList();

            //return Ok(mergedPartResponses);   
        }








    }
}
