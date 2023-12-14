using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Dtos;

namespace SparePart.Services
{
    public interface IPartService
    {
        Task<(IEnumerable<PartForAdditionalInfoDto>?, PaginationMetadata)> GetPartsBySKU(string? sku, int pageSize, int pageNumber);
        Task<(IEnumerable<PartForAdditionalInfoDto>?, PaginationMetadata)> GetAllPartsWithSameCategory(string? sku, int pageSize, int pageNumber);
    }
}
