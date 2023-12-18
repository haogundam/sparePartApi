using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierContact { get; set; } = string.Empty;
        public string? SupplierEmail { get; set; }
        public string SupplierAddress { get; set; } = string.Empty;
    }
}
