using SparePart.ModelAndPersistance.Entities;
using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class WarehouseDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public string WarehouseAddress { get; set; } = string.Empty;
        public ICollection<Storage> Storages { get; set; }
            = new List<Storage>();
    }
}
