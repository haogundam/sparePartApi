using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePart.ModelAndPersistance.Entities
{
    public class Storage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StorageId { get; set; }  
        [ForeignKey("PartId")]
        public Part? Part { get; set; }
        public int PartId {  get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse? Warehouse { get; set; }
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
        public int Quantity { get; set; }

    }
}
