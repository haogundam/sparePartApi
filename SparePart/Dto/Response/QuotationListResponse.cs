using SparePart.ModelAndPersistance;

namespace SparePart.Dto.Response
{
    public class QuotationListResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public ICollection<QuotationResponse> QuotationList { get; set; } = new List<QuotationResponse>();

    }
}
