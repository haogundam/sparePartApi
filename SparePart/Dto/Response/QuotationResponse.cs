using SparePart.ModelAndPersistance.Entities;

namespace SparePart.Dto.Response
{
    public class QuotationResponse
    {
        public int QuoteNo { get; set; }
        public string QuoteDate { get; set; }
        public string QuoteValidDate { get; set; }
        public Status Status { get; set; }
    }
}
