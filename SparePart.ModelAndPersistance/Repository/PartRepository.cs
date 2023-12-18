using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Dtos;
using SparePart.ModelAndPersistance.Entities;
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



        public async Task<int> GetPartQuantity(int partId)
        {
            var totalQuantity = await _context.Storages
                .Where(s => s.PartId == partId)
                .SumAsync(s => s.Quantity);

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
        public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchAllPartsBySKU(string searchQuery, int pageSize, int pageNumber)
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


            var partsToReturn = await collection
              .Include(p => p.Supplier)
              .Include(p => p.Storages)
                     .ThenInclude(s => s.Warehouse)
              .OrderBy(c => c.PartName)
              .SelectMany(p => p.Storages) // Flatten the Storages collection
              .GroupBy(s => new { s.PartId, s.Warehouse.WarehouseId })

              // .GroupBy(s => s.WarehouseId)
              .Select(group => new PartForAdditionalInfoDto
              {
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


            //var partsToReturn =  collection
            //    .Include(a => a.Storages)
            //        .ThenInclude(a=> a.Warehouse)
            //.GroupBy(s=> s.)
            //.Select(p => new PartForAdditionalInfoDto
            //{
            //    PartName = p.PartName,
            //    SupplierName = p.Supplier.SupplierName, // Assuming SupplierName is a property in the Supplier class
            //    TotalQuantity = _context.Storages
            //        .Where(s => s.PartId == p.PartId)
            //        .Sum(s => s.Quantity),
            //    SellingPrice = p.BuyingPrice,
            //    WarehouseName = _context.Storages
            //        .Where(s => s.PartId == p.PartId)
            //        .Select(s => s.Warehouse.WarehouseName)
            //        .FirstOrDefault()
            //})
            //.ToList();



            //Use AutoMapper's ProjectTo for efficient projection
            //var partsToReturn = await collection
            //   .Include(p => p.Supplier)
            //   .Include(p => p.Storages)
            //        .ThenInclude(s => s.Warehouse)
            //    .OrderBy(c => c.PartName)
            //    .SelectMany(p => p.Storages) // Flatten the Storages collection
            //    .GroupBy(s => s.PartId)
            //    .Skip(pageSize * (pageNumber - 1))
            //    .Take(pageSize)
            //    .ProjectTo<PartForAdditionalInfoDto>(_mapper.ConfigurationProvider)  // Assuming you have a PartDto
            //    .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }

        public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> SearchSameCategoryPartsBySKU(string searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
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
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            // Use AutoMapper's ProjectTo for efficient projection
            var partsToReturn = await collection
                .Include(a => a.Storages)
                .Include(a => a.Supplier)
                .OrderBy(c => c.PartName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ProjectTo<PartForAdditionalInfoDto>(_mapper.ConfigurationProvider)  // Assuming you have a PartDto
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }


        public async Task<(IEnumerable<PartForAdditionalInfoDto>, PaginationMetadata)> Testing(string searchQuery, int pageSize, int pageNumber)
        {
            var partsWithWarehouses = await _context.Storages
                .Where(storage => storage.Part.SKU.Contains(searchQuery)) // Filter by SKU
                .Include(storage => storage.Part)
                .Include(storage => storage.Warehouse)
                .OrderBy(storage => storage.Part.PartName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .Select(storage => new PartForAdditionalInfoDto
                {
                    PartName = storage.Part.PartName,
                    SKU = storage.Part.SKU,
                    WarehouseName = storage.Warehouse.WarehouseName,
                    SellingPrice = storage.Part.SellingPrice,
                    TotalQuantity = storage.Quantity,

                    // Add other properties as needed
                })
                .ToListAsync();

            var totalItemCount = await _context.Storages
                .Where(storage => storage.Part.SKU.Contains(searchQuery)) // Count based on the filtered SKU
                .CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            return (partsWithWarehouses, paginationMetadata);
        }




    }
}
