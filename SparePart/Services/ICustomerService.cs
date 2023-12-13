using SparePart.Dto;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;

namespace SparePart.Services
{
    public interface ICustomerService
    {
        Task<bool> CheckCustomerExist(int customerId);

        Task<(IEnumerable<CustomersInfo>?, PaginationMetadata)> SearchCustomerByName(string? name, int pageSize, int pageNumber);

        Task<bool> ValidateCustomerRegistration(RegisterCustomerRequest registerCustomerRequest);
        Task RegisterNewCustomer(RegisterCustomerRequest registerCustomerRequest);




    }
}
