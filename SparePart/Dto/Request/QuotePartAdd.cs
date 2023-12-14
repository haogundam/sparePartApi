namespace SparePart.Dto.Request
{
    public class QuotePartAdd
    {
        public int QuoteNo { get; set; }
        public int PartId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }


    }
}
