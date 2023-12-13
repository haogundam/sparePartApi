using System.ComponentModel.DataAnnotations;

namespace SparePart.ModelAndPersistance.Models
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerContact { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string CustomerAddress { get; set; } = string.Empty;
    }
}
