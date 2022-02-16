
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;
using KadoshShared.Repositories;

namespace KadoshDomain.Services
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleInCashRepository _saleInCashRepository;
        private readonly ISaleInInstallmentsRepository _saleInInstallmentsRepository;
        private readonly ISaleOnCreditRepository _saleOnCreditRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;
        private readonly IInstallmentRepository _installmentRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IStoreRepository _storeRepository;

        public SaleService(
            IUnitOfWork unitOfWork,
            ISaleInCashRepository saleInCashRepository,
            ISaleInInstallmentsRepository saleInInstallmentsRepository,
            ISaleOnCreditRepository saleOnCreditRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IStoreRepository storeRepository,
            ISaleItemRepository saleItemRepository,
            ICustomerPostingRepository customerPostingRepository,
            IInstallmentRepository installmentRepository,
            ISaleRepository saleRepository)
        {
            _unitOfWork = unitOfWork;
            _saleInCashRepository = saleInCashRepository;
            _saleInInstallmentsRepository = saleInInstallmentsRepository;
            _saleOnCreditRepository = saleOnCreditRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _saleItemRepository = saleItemRepository;
            _customerPostingRepository = customerPostingRepository;
            _installmentRepository = installmentRepository;
            _saleRepository = saleRepository;
            _storeRepository = storeRepository;
        }

        public async Task<ICommandResult> CreateSaleInCashAsync(CreateSaleInCashCommand command)
        {
            SaleHandler handler = new(
                _unitOfWork,
                _saleInCashRepository,
                _saleInInstallmentsRepository,
                _saleOnCreditRepository,
                _customerRepository,
                _productRepository,
                _userRepository,
                _saleItemRepository,
                _customerPostingRepository,
                _installmentRepository,
                _storeRepository
                );
            return await handler.HandleAsync(command);
        }

        public async Task<ICommandResult> CreateSaleInInstallmentsAsync(CreateSaleInInstallmentsCommand command)
        {
            SaleHandler handler = new(
                _unitOfWork,
                _saleInCashRepository,
                _saleInInstallmentsRepository,
                _saleOnCreditRepository,
                _customerRepository,
                _productRepository,
                _userRepository,
                _saleItemRepository,
                _customerPostingRepository,
                _installmentRepository,
                _storeRepository
                );
            return await handler.HandleAsync(command);
        }

        public async Task<ICommandResult> CreateSaleOnCreditAsync(CreateSaleOnCreditCommand command)
        {
            SaleHandler handler = new(
                _unitOfWork,
                _saleInCashRepository,
                _saleInInstallmentsRepository,
                _saleOnCreditRepository,
                _customerRepository,
                _productRepository,
                _userRepository,
                _saleItemRepository,
                _customerPostingRepository,
                _installmentRepository,
                _storeRepository
                );
            return await handler.HandleAsync(command);
        }

        public async Task<IEnumerable<Sale>> GetAllSalesIncludingCustomerAsync()
        {
            return await _saleRepository.ReadAllIncludingCustomer();
        }
    }
}
