using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(50, ErrorMessage = "Supplier name cannot exceed 50 characters.")]
        public string SupplierName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Supplier Contact is required.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid phone number")]
        public string SupplierContact { get; set; } = string.Empty;
        [EmailAddress]
        public string? SupplierEmail { get; set; }
        public string SupplierAddress { get; set; } = string.Empty;

        public ICollection<Part> Parts { get; set; }
            = new List<Part>();
    }
}
