using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Models
{
    public class PartForAdditionalInfoDto
    {
        public int PartId { get; set; }
        public string SKU { get; set; }
        public string PartName { get; set; }
        public double SellingPrice { get; set; }
        public double BuyingPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }

    }
}
