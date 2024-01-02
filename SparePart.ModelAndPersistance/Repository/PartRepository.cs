using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public class PartRepository : IPartRepository
    {
        private readonly SparePartContext _context;
        private readonly IMapper _mapper;

        public PartRepository(SparePartContext sparePartContext, IMapper mapper)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
            _mapper = mapper;
        }


        public async Task<Part> GetPartById(int partId)
        {
            var part = await _context.Parts.Where(c => c.PartId == partId).FirstOrDefaultAsync();
            if (part == null)
            {
                throw new InvalidOperationException("Part with the " + partId + "id was not found.");
            }
            return part;
        }


        // TO DO
        public async Task<int> GetPartQuantity(int partId,string warehouseName)
        {
            var parts = await _context.Parts
                .Include(p => p.Storages)
                .ThenInclude(s => s.Warehouse)
                .Where(p => p.PartId == partId)  // Filter by PartId
                .SelectMany(p => p.Storages) // Flatten the Storages collection
                .Where(s => s.Warehouse.WarehouseName == warehouseName)
                .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })
                 .Select(group => new
                 {
                     TotalQuantity = group.Sum(s => s.Quantity)
                 })
                 .FirstOrDefaultAsync();


            int totalQuantity = parts.TotalQuantity;

            return totalQuantity;
        }



        public async Task<string> GetSupplierNameByPartId(int partId)
        {
            var supplierName = await _context.Parts
                    .Where(p => p.PartId == partId)
                    .Select(p => p.Supplier.SupplierName)
                    .FirstOrDefaultAsync();

            return supplierName;
        }

        public async Task<string> GetWarehouseNameByPartId(int partId)
        {
            var warehouseName = await _context.Storages
        .Where(s => s.PartId == partId)
        .Select(s => s.Warehouse.WarehouseName)
        .FirstOrDefaultAsync();

            return warehouseName;
        }



        public async Task<(IEnumerable<Part>, PaginationMetadata)> GetAllParts(int pageSize, int pageNumber)
        {
            var collection = _context.Parts as IQueryable<Part>;

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.PartName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<Part>, PaginationMetadata)> SearchPartsByCategory(string? searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Include(a => a.Category)
                    .Where(a => a.Category != null && a.Category.CategoryName.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.PartName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<Part>, PaginationMetadata)> SearchPartsBySKU(string? searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();


            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Where(a => a.SKU != null && a.SKU.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.PartName)
                .Include(part => part.Category)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<Part>, PaginationMetadata)> GetPartsSameCategoryBySKU(string? searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                var SKUs = collection
                    .Include(a => a.Category)
                    .Where(a => a.SKU.Contains(searchQuery))
                    .ToList();

                var categoryNames = SKUs.Select(a => a.Category.CategoryId).Distinct();
                collection = collection
                    .Include(a => a.Category)
                    .Where(a => categoryNames.Contains(a.Category.CategoryId)
                    && !a.SKU.Contains(searchQuery)
                    );
            }


            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection.OrderBy(c => c.PartName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }


        // NEW
        //public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchAllPartsBySKU(string searchQuery, int pageSize, int pageNumber)
        //{
        //    var collection = _context.Parts.AsQueryable();
        //    var hasAnyMatchingSKU = await collection
        //         .AnyAsync(a => a.SKU != null && a.SKU.Contains(searchQuery));

        //    if (!string.IsNullOrWhiteSpace(searchQuery) && hasAnyMatchingSKU)
        //    {
        //        searchQuery = searchQuery.Trim();
        //        collection = collection
        //            .Where(a => a.SKU != null && a.SKU.Contains(searchQuery));

        //        var totalItemCount = await _context.Storages
        //       .Join(
        //           _context.Parts,
        //           storage => storage.PartId,
        //           part => part.PartId,
        //           (storage, part) => new { storage.PartId, WarehouseName = storage.Warehouse.WarehouseName, SKU = part.SKU }
        //       )
        //       .Where(result => result.SKU.Contains(searchQuery))
        //       .Distinct()
        //       .CountAsync();

        //        var paginationMetadata = new PaginationMetadata(
        //            totalItemCount, pageSize, pageNumber);


        //        var partsToReturn = await collection
        //          .Include(p => p.Supplier)
        //          .Include(p => p.Storages)
        //                 .ThenInclude(s => s.Warehouse)
        //          .OrderBy(c => c.PartName)
        //          .SelectMany(p => p.Storages) // Flatten the Storages collection
        //          .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })
        //          .Select(group => new PartForAdditionalInfoDto
        //          {
        //              PartId = group.First().Part.PartId,
        //              SKU = group.First().Part.SKU,
        //              PartName = group.First().Part.PartName,
        //              SellingPrice = group.First().Part.SellingPrice,
        //              SupplierName = group.First().Part.Supplier.SupplierName,
        //              WarehouseName = group.First().Warehouse.WarehouseName,
        //              TotalQuantity = group.Sum(s => s.Quantity)
        //          })
        //          .Skip(pageSize * (pageNumber - 1))
        //          .Take(pageSize)
        //          .ToListAsync();

        //        return (partsToReturn, paginationMetadata);
        //    }
        //    else if (!string.IsNullOrWhiteSpace(searchQuery) && !hasAnyMatchingSKU)
        //    {
        //        return (null, null);
        //    }
        //    else
        //    {
        //        var totalItemCount = await _context.Storages
        //       .Join(
        //           _context.Parts,
        //           storage => storage.PartId,
        //           part => part.PartId,
        //           (storage, part) => new { storage.PartId, WarehouseName = storage.Warehouse.WarehouseName, SKU = part.SKU }
        //       )
        //       //.Where(result => result.SKU.Contains(searchQuery))
        //       .Distinct()
        //       .CountAsync();

        //        var paginationMetadata = new PaginationMetadata(
        //            totalItemCount, pageSize, pageNumber);


        //        var partsToReturn = await collection
        //          .Include(p => p.Supplier)
        //          .Include(p => p.Storages)
        //                 .ThenInclude(s => s.Warehouse)
        //          .OrderBy(c => c.PartName)
        //          .SelectMany(p => p.Storages) // Flatten the Storages collection
        //          .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })
        //          .Select(group => new PartForAdditionalInfoDto
        //          {
        //              PartId = group.First().Part.PartId,
        //              SKU = group.First().Part.SKU,
        //              PartName = group.First().Part.PartName,
        //              SellingPrice = group.First().Part.SellingPrice,
        //              SupplierName = group.First().Part.Supplier.SupplierName,
        //              WarehouseName = group.First().Warehouse.WarehouseName,
        //              TotalQuantity = group.Sum(s => s.Quantity)
        //          })
        //          .Skip(pageSize * (pageNumber - 1))
        //          .Take(pageSize)
        //          .ToListAsync();

        //        return (partsToReturn, paginationMetadata);

        //    }

        //}

        public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchAllPartsBySKU(string searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection
                    .Where(a => a.SKU != null && a.SKU.Contains(searchQuery));

                if (!await collection.AnyAsync())
                {
                    return (null, null);
                }
            }

            var totalItemCount = await _context.Storages
                .Join(
                    _context.Parts,
                    storage => storage.PartId,
                    part => part.PartId,
                    (storage, part) => new { storage.PartId, WarehouseName = storage.Warehouse.WarehouseName, SKU = part.SKU }
                )
                .Where(result => string.IsNullOrWhiteSpace(searchQuery) || result.SKU.Contains(searchQuery))
                .Distinct()
                .CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var partsToReturn = await collection
                .Include(p => p.Supplier)
                .Include(p => p.Storages)
                .ThenInclude(s => s.Warehouse)
                .OrderBy(c => c.PartName)
                .SelectMany(p => p.Storages)
                .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })
                .Select(group => new PartForAdditionalInfoDto
                {
                    PartId = group.First().Part.PartId,
                    SKU = group.First().Part.SKU,
                    PartName = group.First().Part.PartName,
                    SellingPrice = group.First().Part.SellingPrice,
                    SupplierName = group.First().Part.Supplier.SupplierName,
                    WarehouseName = group.First().Warehouse.WarehouseName,
                    TotalQuantity = group.Sum(s => s.Quantity)
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);

        }




        public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchSameCategoryPartsBySKU(string searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();
            var hasAnyMatchingSKU = await collection
                 .AnyAsync(a => a.SKU != null && a.SKU.Contains(searchQuery));

            if (!string.IsNullOrWhiteSpace(searchQuery) && hasAnyMatchingSKU)
            {
                searchQuery = searchQuery.Trim();
                var SKUs = collection
                    .Include(a => a.Category)
                    .Where(a => a.SKU != null && a.SKU.Contains(searchQuery));

                var categoryNames = SKUs.Select(a => a.Category.CategoryId).Distinct();
                collection = collection
                    .Include(a => a.Category)
                    .Where(a => categoryNames.Contains(a.Category.CategoryId)
                    && !a.SKU.Contains(searchQuery)
                    );


                var totalItemCount = await collection
                 .Join(
                   _context.Storages,
                   part => part.PartId,
                   storage => storage.PartId,
                   (part, storage) => new { part.PartId, storage.Warehouse.WarehouseName, part.SKU }
                  )
               .Where(result => !result.SKU.Contains(searchQuery))
               .Distinct()
               .CountAsync();

                if (totalItemCount == 0)
                {
                    return (null, null);
                }

                var paginationMetadata = new PaginationMetadata(
                    totalItemCount, pageSize, pageNumber);

                // Use AutoMapper's ProjectTo for efficient projection
                var partsToReturn = await collection
                  .Include(p => p.Supplier)
                  .Include(p => p.Storages)
                         .ThenInclude(s => s.Warehouse)
                  .OrderBy(c => c.PartName)
                  .SelectMany(p => p.Storages) // Flatten the Storages collection
                  .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })
                  .Select(group => new PartForAdditionalInfoDto
                  {
                      PartId = group.First().Part.PartId,
                      SKU = group.First().Part.SKU,
                      PartName = group.First().Part.PartName,
                      SellingPrice = group.First().Part.SellingPrice,
                      SupplierName = group.First().Part.Supplier.SupplierName,
                      WarehouseName = group.First().Warehouse.WarehouseName,
                      TotalQuantity = group.Sum(s => s.Quantity)
                  })
                  .Skip(pageSize * (pageNumber - 1))
                  .Take(pageSize)
                  .ToListAsync();

                return (partsToReturn, paginationMetadata);

            }
            else
            {
                return (null, null);
            }



        }






    }
}
