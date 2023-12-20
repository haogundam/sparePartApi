using SparePart.ModelAndPersistance;

namespace SparePart.Dto.Response
{
    // Return One List
    //public class QuotationListResponse
    //{
    //    public int CustomerId { get; set; }
    //    public string CustomerName { get; set; } = string.Empty;
    //    public ICollection<QuotationResponse> QuotationList { get; set; } = new List<QuotationResponse>();

    //}

    // Return Two List
    public class QuotationListResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public ICollection<QuotationResponse> PendingQuotationList { get; set; } = new List<QuotationResponse>();
        public ICollection<QuotationResponse> PaidQuotationList { get; set; } = new List<QuotationResponse>();

    }


}
