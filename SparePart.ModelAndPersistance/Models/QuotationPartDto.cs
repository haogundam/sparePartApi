using SparePart.ModelAndPersistance.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class QuotationPartDto
    {
        public int QuoteNo { get; set; }
        public int PartId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
