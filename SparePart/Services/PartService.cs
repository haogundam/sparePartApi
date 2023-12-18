using AutoMapper;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartService(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }



        public async Task<(IEnumerable<PartForAdditionalInfoDto>?, PaginationMetadata)> GetPartsBySKU(string? sku, int pageSize, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.SearchAllPartsBySKU(sku, pageSize, pageNumber);
            if (parts == null)
            {
                return (null, paginationMetadata);
            }
            return (parts, paginationMetadata);

        }
        public async Task<(IEnumerable<PartForAdditionalInfoDto>?,PaginationMetadata)> GetAllPartsWithSameCategory(string? sku,int pageSize, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.SearchSameCategoryPartsBySKU(sku, pageSize, pageNumber);
            
            if (parts == null)
            {
                return (null, paginationMetadata);
            }

            return (parts, paginationMetadata);

        }

    }
}
