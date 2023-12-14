using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePart.ModelAndPersistance.Dtos
{
    public class PartForAdditionalInfoDto
    {
        public string SKU { get; set; }
        public string PartName { get; set; }
        public double SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
        public string SupplierName { get; set; }
        public string WarehouseName { get; set; }

    }
}
