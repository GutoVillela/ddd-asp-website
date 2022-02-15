using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Repositories;

namespace KadoshDomain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<ICommandResult> CreateCustomerAsync(CreateCustomerCommand command)
        {
            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCustomerAsync(DeleteCustomerCommand command)
        {
            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.ReadAllAsync();
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _customerRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateCustomerAsync(UpdateCustomerCommand command)
        {
            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
        }
    }
}
