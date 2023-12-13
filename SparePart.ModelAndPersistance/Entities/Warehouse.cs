using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = "Warehouse name is required.")]
        [StringLength(50, ErrorMessage = "Warehouse name cannot exceed 50 characters.")]
        public string WarehouseName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Warehouse Address is required.")]
        public string WarehouseAddress { get; set; } = string.Empty;
        public ICollection<Storage> Storages { get; set; }
            = new List<Storage>();

    }
}
