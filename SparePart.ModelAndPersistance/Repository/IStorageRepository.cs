using SparePart.ModelAndPersistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Repository
{
    public interface IStorageRepository
    {
        Task UpdateStorageQuantity(QuotationPart quotationPart, int quantity);

        Task IncreaseStorageQuantity(int partId, int quantity);

        Task DecreaseStorageQuantity(QuotationPart quotationPart, int quantity);

    }
}
