﻿namespace SparePart.Dto.Response
{
    public class PartsInQuotationList
    {
        public int PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; } 
        public double UnitPrice { get; set; }
    }
}
