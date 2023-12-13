using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string CustomerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Customer contact is required.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid phone number")]
        public string CustomerContact { get; set; } = string.Empty;
        [EmailAddress]
        public string? CustomerEmail { get; set; }
        public string CustomerAddress { get; set; } = string.Empty;

        public ICollection<QuotationList> QuotationLists { get; set; }
            = new List<QuotationList>();

    }
}
