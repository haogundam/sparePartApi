using System.ComponentModel.DataAnnotations;

namespace SparePart.Dto.Response
{
    public class CustomersInfo
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerContact { get; set; } = string.Empty;

        public string? CustomerEmail { get; set; }

    }

}
