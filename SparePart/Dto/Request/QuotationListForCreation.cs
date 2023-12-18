using SparePart.ModelAndPersistance.Entities;
using SparePart.ModelAndPersistance;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SparePart.Dto.Request
{
    public class QuotationListForCreation
    {
        public DateOnly QuoteDate { get; set; }
        public DateOnly QuoteValidDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; } = 1;
        public double TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public Status Status { get; set; }
        public ICollection<QuotationPart> QuotationParts { get; set; }
            = new List<QuotationPart>();
    }
}
