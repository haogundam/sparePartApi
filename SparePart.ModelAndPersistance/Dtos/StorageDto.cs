using SparePart.ModelAndPersistance.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class StorageDto
    {
        public int StorageId { get; set; }
        public int PartId { get; set; }
        public int WarehouseId { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
