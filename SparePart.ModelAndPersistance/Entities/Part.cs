using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class Part
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartId { get; set; }
        [Required(ErrorMessage = "SKU is required.")]
        public string SKU { get; set; } = string.Empty;
        [Required(ErrorMessage = "Part name is required.")]
        [StringLength(50, ErrorMessage = "Part name cannot exceed 50 characters.")]
        public string PartName { get; set; } = string.Empty;
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Selling Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "SellingPrice must be a positive number")]
        public double SellingPrice { get; set; }
        [Required(ErrorMessage = "Buying Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "BuyingPrice must be a positive number")]
        public double BuyingPrice { get; set; }
        [Required(ErrorMessage = "Date of Entry is required.")]
        public DateOnly EntryDate { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }
        public int SupplierId { get; set; }
        public ICollection<QuotationPart> QuotationParts { get; set; }
            = new List<QuotationPart>();
        public ICollection<Storage> Storages { get; set; }
            = new List<Storage>();

    }
}
