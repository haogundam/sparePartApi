using Microsoft.EntityFrameworkCore;
using SparePart.ModelAndPersistance.Context;
using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public class PartRepository : IPartRepository
    {
        private readonly SparePartContext _context;

        public PartRepository(SparePartContext sparePartContext)
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
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

        public async Task<(IEnumerable<Part>, PaginationMetadata)> SearchPartsBySKU(string searchQuery, int pageSize, int pageNumber)
        {
            var collection = _context.Parts.AsQueryable();
         //       .Include(part => part.Category);

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
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (partsToReturn, paginationMetadata);
        }
    }
}
