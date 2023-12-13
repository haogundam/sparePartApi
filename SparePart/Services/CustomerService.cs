using AutoMapper;
using SparePart.Dto;
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

            if (exists == null) { return false; }

            return true;
        }

        public async Task RegisterNewCustomer(RegisterCustomerRequest registerCustomerRequest)
        {
            var register = _mapper.Map<ModelAndPersistance.Entities.Customer>(registerCustomerRequest);
            await _customerRepository.AddCustomer(register);
        }



    }
}
