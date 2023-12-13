using SparePart.Dto;

namespace SparePart.Services
{
    public interface ICustomerService
    {
        Task<bool> CheckCustomerExist(int customerId);

        Task RegisterNewCustomer(RegisterCustomerRequest registerCustomerRequest);




    }
}
