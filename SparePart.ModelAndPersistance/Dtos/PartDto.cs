using SparePart.ModelAndPersistance.Models;

namespace SparePart.ModelAndPersistance.DTOs
{
    public class PartDto
    {
        public int PartId { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public double SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
        public double BuyingPrice { get; set; }
        public DateOnly EntryDate { get; set; }

        public ICollection<QuotationPartDto> QuotationParts { get; set; }
            = new List<QuotationPartDto>();

        public ICollection<StorageDto> Storages { get; set; }
            = new List<StorageDto>();
    }
}
