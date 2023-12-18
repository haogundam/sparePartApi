using SparePart.ModelAndPersistance.Dtos;
using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public interface IPartRepository
    {
        Task<Part> GetPartById(int partId);



        Task<(IEnumerable<Part>, PaginationMetadata)> GetAllParts(int pageSize, int pageNumber);

        Task<(IEnumerable<Part>, PaginationMetadata)> SearchPartsByCategory(string? searchQuery, int pageSize, int pageNumber);

        Task<int> GetPartQuantity(int partId);

        Task<string> GetSupplierNameByPartId(int partId);

        Task<string> GetWarehouseNameByPartId(int partId);
       

        Task<(IEnumerable<Part>, PaginationMetadata)> SearchPartsBySKU(string? searchQuery, int pageSize, int pageNumber);

        Task<(IEnumerable<Part>, PaginationMetadata)> GetPartsSameCategoryBySKU(string? searchQuery, int pageSize, int pageNumber);


        Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchAllPartsBySKU(string searchQuery, int pageSize, int pageNumber);
        Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchSameCategoryPartsBySKU(string searchQuery, int pageSize, int pageNumber);


        Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> Testing(string searchQuery, int pageSize, int pageNumber);

        }
}
