using AutoMapper;
using SparePart.Dto;
using SparePart.Dto.Response;
using SparePart.ModelAndPersistance;
using SparePart.ModelAndPersistance.Repository;

namespace SparePart.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository,IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<bool> CheckCustomerExist(int customerId)
        {
            var exists = await _customerRepository.CustomerExistsAsync(customerId);

            if (exists == false) { return false; }

            return true;
        }

        public async Task<(IEnumerable<CustomersInfo>?,PaginationMetadata)> SearchCustomerByName(string? name,int pageSize, int pageNumber)
        {
            var (customerEntities, paginationMetadata) = await _customerRepository.SearchCustomerByCustomerName(name, pageSize, pageNumber);

            if (customerEntities == null)
            {
                return (null, paginationMetadata);
            }

            return( _mapper.Map<IEnumerable<CustomersInfo>>(customerEntities), paginationMetadata);

        }


        public async Task<bool> ValidateCustomerRegistration(RegisterCustomerRequest registerCustomerRequest)
        {
            if (registerCustomerRequest.CustomerName == null || registerCustomerRequest.CustomerContact == null || 
                registerCustomerRequest.CustomerEmail == null || registerCustomerRequest.CustomerAddress == null)
            {
                return (false);
            }

            return true;
        }
        public async Task RegisterNewCustomer(RegisterCustomerRequest registerCustomerRequest)
        {
            var register = _mapper.Map<ModelAndPersistance.Entities.Customer>(registerCustomerRequest);
            await _customerRepository.AddCustomer(register);
        }



    }
}
