using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class QuotationListDto
    {
        public int QuoteNo { get; set; }
        public DateOnly QuoteDate { get; set; }
        public DateOnly QuoteValidDate { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public string Status { get; set; } = "pending";
        public ICollection<QuotationPartDto> QuotationParts { get; set; }
            = new List<QuotationPartDto>();
    }
}
