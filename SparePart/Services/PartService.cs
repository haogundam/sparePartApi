using AutoMapper;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Dtos;
using SparePart.ModelAndPersistance.Entities;
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

            //var (parts, paginationMetadata) = await _partRepository.SearchPartsBySKU(sku, pageSize, pageNumber);
            //if (parts == null) { return (null, paginationMetadata); }

            //var skuPartResponses = new List<PartResponse>();

            //foreach (var part in parts)
            //{
            //    //var supplierName = await _partRepository.GetSupplierNameByPartId(part.PartId);
            //    //var warehouseName = await _partRepository.GetWarehouseNameByPartId(part.PartId);
            //    //var quantityLeft = await _partRepository.GetPartQuantity(part.PartId);

            //    var partResponse = _mapper.Map<PartResponse>(part);

            //    //partResponse.SupplierName = supplierName;
            //    //partResponse.WarehouseName = warehouseName;
            //   // partResponse.Quantity = quantityLeft;

            //    skuPartResponses.Add(partResponse);
            //}
            // return (skuPartResponses, paginationMetadata);
        }
        public async Task<(IEnumerable<PartForAdditionalInfoDto>?,PaginationMetadata)> GetAllPartsWithSameCategory(string? sku,int pageSize, int pageNumber)
        {
            var (parts, paginationMetadata) = await _partRepository.SearchSameCategoryPartsBySKU(sku, pageSize, pageNumber);
            if (parts == null)
            {
                return (null, paginationMetadata);
            }
            return (parts, paginationMetadata);



            //var (parts, paginationMetadata) = await _partRepository.GetPartsSameCategoryBySKU(sku, pageSize, pageNumber);
            //if (parts == null) { return  (null, paginationMetadata); }

            //var skuPartResponses = new List<PartResponse>();

            //foreach (var part in parts)
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

            //return (skuPartResponses,paginationMetadata);
        }

    }
}
