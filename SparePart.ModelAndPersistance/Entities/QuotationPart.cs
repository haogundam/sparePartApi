using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class QuotationPart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuotePartId { get; set; }
        [ForeignKey("QuoteNo")]
        public int QuoteNo { get; set; }
        public QuotationList? QuotationList { get; set; }
        [ForeignKey("PartId")]
        public int PartId { get; set; }
        public Part? Part { get; set; }
        [Required(ErrorMessage = "Unit Price of the Part is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "UnitPrice must be greater than 0")]
        public double UnitPrice { get; set; }
        [Required(ErrorMessage = "Quantity of the Part is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
