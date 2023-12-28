using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class QuotationList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuoteNo { get; set; }
        [Required(ErrorMessage = "Date of Quotation is required.")]
        public DateOnly QuoteDate { get; set; }
        [Required(ErrorMessage = "Date of Valid Quotation is required.")]
        public DateOnly QuoteValidDate { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Total Amount of the Quotation is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be a positive number")]
        public double TotalAmount { get; set; }
        [Required(ErrorMessage = "Payment Type of the Quotation is required.")]
        public string? PaymentType { get; set; }
        [Required(ErrorMessage = "The Status of the transaction is required.")]
        public Status Status { get; set; }
        public ICollection<QuotationPart> QuotationParts { get; set; }
            = new List<QuotationPart>();
    }
}
