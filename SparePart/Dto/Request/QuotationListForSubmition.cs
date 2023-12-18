using SparePart.ModelAndPersistance.Entities;

namespace SparePart.Dto.Request
{
    public class QuotationListForSubmition
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
