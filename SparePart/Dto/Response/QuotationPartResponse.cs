namespace SparePart.Dto.Response
{
    public class QuotationPartResponse
    {
        public int QuoteNo { get; set; }
        public string QuoteDate { get; set; }
        public double TotalAmount { get; set; }
        
        public ICollection<PartsInQuotationList> Parts { get; set; } = new List<PartsInQuotationList>();


    }
}
