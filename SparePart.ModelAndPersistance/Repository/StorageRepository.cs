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
    public class StorageRepository : IStorageRepository
    {
        private readonly SparePartContext _context;

        public StorageRepository(SparePartContext sparePartContext) 
        {
            _context = sparePartContext ?? throw new ArgumentNullException(nameof(sparePartContext));
        }

        public async Task UpdateStorageQuantity(QuotationPart quotationPart, int quantity) 
        {
            if (quotationPart != null)
            {
                // Retrieve the associated Storage
                var storageToUpdate = await _context.Storages
                    .OrderBy(s => s.WarehouseId)
                    .FirstOrDefaultAsync(s => s.PartId == quotationPart.PartId);

                if (storageToUpdate != null)
                {
                    int currentQuantity = storageToUpdate.Quantity;

                    // Determine whether to increase or decrease storage quantity
                    if (quantity > currentQuantity)
                    {
                        // Decrease storage quantity
                        await DecreaseStorageQuantity(quotationPart, quantity - currentQuantity);
                    }
                    else if (quantity < currentQuantity)
                    {
                        // Increase storage quantity
                        await IncreaseStorageQuantity(quotationPart.PartId, currentQuantity - quantity);
                    }

                    // Update the Storage quantity to the new quantity
                    storageToUpdate.Quantity = quantity;
                    await _context.SaveChangesAsync();
                }
            }


        }
        public async Task IncreaseStorageQuantity(int partId,int quantity)
        {
            
                // Retrieve the associated Storage
                var storageToUpdate = await _context.Storages
                    .OrderBy(s => s.WarehouseId)
                    .FirstOrDefaultAsync(s => s.PartId == partId);

                if (storageToUpdate != null)
                {
                    // Update the Storage quantity by adding the quantity of the removed QuotationPart
                    storageToUpdate.Quantity += quantity;
                    await _context.SaveChangesAsync();
                }
            
        }
        public async Task DecreaseStorageQuantity(QuotationPart quotationPart, int quantity) 
        {
            if (quotationPart != null)
            {
                // Retrieve the associated Storage
                var storagesToUpdate = await _context.Storages
                    .Where(s => s.PartId == quotationPart.PartId)
                    .OrderBy(s => s.WarehouseId)
                    .ToListAsync();

                foreach (var storageToUpdate in storagesToUpdate)
                {
                    if (quantity > 0)
                    {
                        int deductionAmount = Math.Min(quantity, storageToUpdate.Quantity);
                        storageToUpdate.Quantity -= deductionAmount;
                        quantity -= deductionAmount;
                    }
                    else
                    {
                        break; // No more quantity to deduct
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

    










    }
}
