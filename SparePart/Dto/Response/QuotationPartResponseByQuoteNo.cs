using SparePart.ModelAndPersistance.Entities;

namespace SparePart.Dto.Response
{
    public class QuotationPartResponseByQuoteNo
    {
        public CustomersInfo CustomersInfo { get; set; }

        public Status Status { get; set; }
        public int QuoteNo { get; set; }
        public string QuoteDate { get; set; }
        public double TotalAmount { get; set; }
        public ICollection<PartsInQuotationList> Parts { get; set; } = new List<PartsInQuotationList>();

    }
}
