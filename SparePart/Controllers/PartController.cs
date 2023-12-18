using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Repository;
using SparePart.Services;

namespace SparePart.Controllers
{
    [Route("api/parts")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;
        private readonly IPartRepository _partRepository;
        const int PageSize = 10;
        public PartController(IPartService partService, IPartRepository partRepository)
        {
            _partService = partService;
            _partRepository = partRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartResponse>>> GetAllParts(string? sku, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.SearchAllPartsBySKU(sku, PageSize, pageNumber);
            if (parts == null) { return NotFound(); }


            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1)
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (parts, paginationMetadata) = await _partRepository.SearchAllPartsBySKU(sku, PageSize, pageNumber);
            }


            Response.Headers.Add("X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(parts);

        }

        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<PartResponse>>> GetSameCategoryParts(string? sku, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partService.GetAllPartsWithSameCategory(sku, PageSize, pageNumber);
            if (parts == null) { return NoContent(); }

            if (pageNumber > paginationMetadata.TotalPageCount || pageNumber < 1)
            {
                pageNumber = paginationMetadata.TotalPageCount;
                (parts, paginationMetadata) = await _partService.GetAllPartsWithSameCategory(sku, PageSize, pageNumber);
            }

            if (parts == null) { return NoContent(); }

            Response.Headers.Add("X-Pagination",
                 System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(parts);
        }
    }
}
