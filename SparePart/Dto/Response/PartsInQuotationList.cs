namespace SparePart.Dto.Response
{
    public class PartsInQuotationList
    {
        public int QuotePartId { get; set; }
        public int PartId { get; set; }
        public string SKU { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; } 
        public double UnitPrice { get; set; }
    }
}
