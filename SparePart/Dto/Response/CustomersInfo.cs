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

    //public class CustomerInfoResponse 
    //{
    
    //    public ICollection<CustomersInfo> CustomersInfo { get; set; }
    //    public int  TotalPageCount { get; set; }
    
    //}

}
