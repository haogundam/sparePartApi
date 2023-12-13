namespace SparePart.Dto.Response
{
    public class PartResponse
    {
        public int PartId { get; set; }
        public string SKU { get; set; }
        public string PartName { get; set; }
        public double SellingPrice { get; set; }

        public string SupplierName { get; set; }

        public int Quantity { get; set; }
        

        public string WarehouseName { get; set; }

        public int NewQuantity { get; set; } = 0;
        public double NewPrice { get; set; } = 0.0;
    }

}
