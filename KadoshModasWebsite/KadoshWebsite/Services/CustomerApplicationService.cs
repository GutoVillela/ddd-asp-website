using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Handlers;
using KadoshDomain.Commands.CustomerCommands.CreateCustomer;
using KadoshDomain.Commands.CustomerCommands.DeleteCustomer;
using KadoshDomain.Commands.CustomerCommands.UpdateCustomer;
using KadoshDomain.Commands.CustomerCommands.InformPayment;
using KadoshDomain.Queries.CustomerQueries.GetAllCustomers;
using KadoshShared.ExtensionMethods;
using KadoshDomain.Queries.CustomerQueries.GetCustomerById;
using KadoshDomain.Queries.CustomerQueries.DTOs;
using KadoshDomain.Queries.CustomerQueries.GetCustomerTotalDebt;
using KadoshWebsite.Infrastructure;
using KadoshDomain.Queries.CustomerQueries.GetCustomersByName;
using KadoshDomain.Queries.CustomerQueries.GetCustomerByUsername;
using KadoshShared.Constants.ErrorCodes;
using KadoshDomain.Commands.CustomerCommands.CreateCustomerUser;

namespace KadoshWebsite.Services
{
    public class CustomerApplicationService : ICustomerApplicationService
    {
        private readonly ICommandHandler<CreateCustomerCommand> _createCustomerHandler;
        private readonly ICommandHandler<DeleteCustomerCommand> _deleteCustomerHandler;
        private readonly ICommandHandler<UpdateCustomerCommand> _updateCustomerHandler;
        private readonly ICommandHandler<InformPaymentCommand> _informPaymentHandler;
        private readonly ICommandHandler<CreateCustomerUserCommand> _createCustomerUserHandler;
        private readonly IQueryHandler<GetAllCustomersQuery, GetAllCustomersQueryResult> _getAllCustomersQueryHandler;
        private readonly IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult> _getCustomerByIdQueryHandler;
        private readonly IQueryHandler<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult> _getCustomerTotalDebtQueryHandler;
        private readonly IQueryHandler<GetCustomersByNameQuery, GetCustomersByNameQueryResult> _getCustomersByNameQueryHandler;
        private readonly IQueryHandler<GetCustomerByUsernameQuery, GetCustomerByUsernameQueryResult> _getCustomerByUsernameQueryHandler;

        public CustomerApplicationService(
            ICommandHandler<CreateCustomerCommand> createCustomerHandler,
            ICommandHandler<DeleteCustomerCommand> deleteCustomerHandler,
            ICommandHandler<UpdateCustomerCommand> updateCustomerHandler,
            ICommandHandler<InformPaymentCommand> informPaymentHandler,
            ICommandHandler<CreateCustomerUserCommand> createCustomerUserHandler,
            IQueryHandler<GetAllCustomersQuery, GetAllCustomersQueryResult> getAllCustomersQueryHandler,
            IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult> getCustomerByIdQueryHandler,
            IQueryHandler<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult> getCustomerTotalDebtQueryHandler,
            IQueryHandler<GetCustomersByNameQuery, GetCustomersByNameQueryResult> getCustomersByNameQueryHandler,
            IQueryHandler<GetCustomerByUsernameQuery, GetCustomerByUsernameQueryResult> getCustomerByUsernameQueryHandler)
        {
            _createCustomerHandler = createCustomerHandler;
            _deleteCustomerHandler = deleteCustomerHandler;
            _updateCustomerHandler = updateCustomerHandler;
            _informPaymentHandler = informPaymentHandler;
            _createCustomerUserHandler = createCustomerUserHandler;
            _getAllCustomersQueryHandler = getAllCustomersQueryHandler;
            _getCustomerByIdQueryHandler = getCustomerByIdQueryHandler;
            _getCustomerTotalDebtQueryHandler = getCustomerTotalDebtQueryHandler;
            _getCustomersByNameQueryHandler = getCustomersByNameQueryHandler;
            _getCustomerByUsernameQueryHandler = getCustomerByUsernameQueryHandler;
        }

        public async Task<ICommandResult> CreateCustomerAsync(CustomerViewModel customer)
        {
            CreateCustomerCommand command = new();
            command.Name = customer.Name;
            command.EmailAddress = customer.Email;

            if (string.IsNullOrEmpty(customer.DocumentNumber))
            {
                command.DocumentType = customer.DocumentType;
                command.DocumentNumber = customer.DocumentNumber;
            }

            if(customer.Gender != EGender.NotDefined)
                command.Gender = customer.Gender;

            customer.Street = customer.Street;
            customer.Number = customer.Number;
            customer.Neighborhood = customer.Neighborhood;
            customer.City = customer.City;
            customer.State = customer.State;
            customer.ZipCode = customer.ZipCode;
            customer.Complement = customer.Complement;

            if(customer.Phones.Any())
                command.Phones = new List<Phone>();

            foreach(var phone in customer.Phones)
                command.Phones.Add(ConvertPhoneModelToValueObject(phone));

            return await _createCustomerHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCustomerAsync(int id)
        {
            DeleteCustomerCommand command = new();
            command.Id = id;

            return await _deleteCustomerHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
        {
            var result = await _getAllCustomersQueryHandler.HandleAsync(new GetAllCustomersQuery());

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var customerViewModels = new List<CustomerViewModel>();

            foreach (var customer in result.Customers)
            {
                customerViewModels.Add(GetCustomerViewModelFromDTO(customer));
            }
            return customerViewModels;
        }

        public async Task<PaginatedListViewModel<CustomerViewModel>> GetAllCustomersPaginatedAsync(int currentPage, int pageSize, bool includeInactive)
        {
            GetAllCustomersQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;
            query.IncludeInactives = includeInactive;

            var result = await _getAllCustomersQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var customerViewModels = new List<CustomerViewModel>();

            foreach (var customer in result.Customers)
            {
                customerViewModels.Add(GetCustomerViewModelFromDTO(customer));
            }

            PaginatedListViewModel<CustomerViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomersCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomersCount, pageSize);
            paginatedList.Items = customerViewModels;

            return paginatedList;
        }
        public async Task<CustomerViewModel> GetCustomerAsync(int id)
        {
            GetCustomerByIdQuery query = new();
            query.CustomerId = id;

            var result = await _getCustomerByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            CustomerViewModel viewModel = GetCustomerViewModelFromDTO(result.Customer!);

            viewModel.Phones = GetPhonesFromDTOAsViewModels(result.Customer!);

            return viewModel;
        }

        public async Task<ICommandResult> UpdateCustomerAsync(CustomerViewModel customer)
        {
            UpdateCustomerCommand command = new();
            command.Id = customer.Id;
            command.Name = customer.Name;
            command.EmailAddress = customer.Email;

            if (string.IsNullOrEmpty(customer.DocumentNumber))
            {
                command.DocumentType = customer.DocumentType;
                command.DocumentNumber = customer.DocumentNumber;
            }

            if (customer.Gender != EGender.NotDefined)
                command.Gender = customer.Gender;

            customer.Street = customer.Street;
            customer.Number = customer.Number;
            customer.Neighborhood = customer.Neighborhood;
            customer.City = customer.City;
            customer.State = customer.State;
            customer.ZipCode = customer.ZipCode;
            customer.Complement = customer.Complement;

            if (customer.Phones.Any())
                command.Phones = new List<Phone>();

            foreach (var phone in customer.Phones)
                command.Phones.Add(ConvertPhoneModelToValueObject(phone));

            return await _updateCustomerHandler.HandleAsync(command);
        }

        public async Task<decimal> GetCustomerTotalDebtAsync(int customerId)
        {
            GetCustomerTotalDebtQuery query = new();
            query.CustomerId = customerId;

            var result = await _getCustomerTotalDebtQueryHandler.HandleAsync(query);

            if(!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            return result.CustomerTotalDebt;
        }

        public async Task<ICommandResult> InformCustomerPaymentAsync(int customerId, decimal amountToInform)
        {
            InformPaymentCommand command = new();
            command.CustomerId = customerId;
            command.AmountToInform = amountToInform;

            return await _informPaymentHandler.HandleAsync(command);
        }

        public async Task<PaginatedListViewModel<CustomerViewModel>> GetAllCustomersByNamePaginatedAsync(string customerName, int currentPage, int pageSize, bool includeInactive)
        {
            GetCustomersByNameQuery query = new();
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;
            query.CustomerName = customerName;
            query.IncludeInactives = includeInactive;

            var result = await _getCustomersByNameQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            var customerViewModels = new List<CustomerViewModel>();

            foreach (var customer in result.Customers)
            {
                customerViewModels.Add(GetCustomerViewModelFromDTO(customer));
            }

            PaginatedListViewModel<CustomerViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomersCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomersCount, pageSize);
            paginatedList.Items = customerViewModels;

            return paginatedList;
        }

        public async Task<CustomerViewModel?> GetCustomerUserByUsernameAsync(string username)
        {
            GetCustomerByUsernameQuery query = new();
            query.Username = username;

            var result = await _getCustomerByUsernameQueryHandler.HandleAsync(query);

            if (!result.Success)
            {
                if(result.Errors.Any(x => x.Code == ErrorCodes.ERROR_CUSTOMER_USERNAME_NOT_FOUND))
                    return null;

                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            }

            CustomerViewModel viewModel = GetCustomerViewModelFromDTO(result.Customer!);

            viewModel.Phones = GetPhonesFromDTOAsViewModels(result.Customer!);

            return viewModel;
        }

        public async Task<ICommandResult> CreateCustomerUserAsync(int customerId, string username, string password)
        {
            CreateCustomerUserCommand command = new();
            command.CustomerId = customerId;
            command.Username = username;
            command.Password = password;

            return await _createCustomerUserHandler.HandleAsync(command);
        }

        private CustomerViewModel GetCustomerViewModelFromDTO(CustomerDTO customerDTO)
        {
            return new()
            {
                Id = customerDTO.Id,
                Name = customerDTO.Name,
                Gender = customerDTO.Gender,
                Email = customerDTO.Email,
                DocumentType = customerDTO.DocumentType,
                DocumentNumber = customerDTO.DocumentNumber,
                Street = customerDTO.Street,
                Number = customerDTO.Number,
                Neighborhood = customerDTO.Neighborhood,
                City = customerDTO.City,
                State = customerDTO.State,
                ZipCode = customerDTO.ZipCode,
                Complement = customerDTO.Complement,
                Username = customerDTO.Username,
            };
        }

        private Phone ConvertPhoneModelToValueObject(PhoneViewModel phoneViewModel)
        {
            string fullPhoneNumber = phoneViewModel.PhoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            string areaCode = fullPhoneNumber[..2];
            string phoneNumber = fullPhoneNumber[2..];

            if (phoneViewModel.Id.HasValue)
                return new Phone(id: phoneViewModel.Id.Value, areaCode: areaCode, number: phoneNumber, type: phoneViewModel.PhoneType, talkTo: phoneViewModel.TalkTo);
            else
                return new Phone(areaCode: areaCode, number: phoneNumber, type: phoneViewModel.PhoneType, talkTo: phoneViewModel.TalkTo);
        }

        private IEnumerable<PhoneViewModel> GetPhonesFromDTOAsViewModels(CustomerDTO customer)
        {
            List<PhoneViewModel> phones = new List<PhoneViewModel>();
            if (customer.Phones is not null && customer.Phones.Any())
            {
                foreach (var phone in customer.Phones)
                {
                    phones.Add(ConvertPhoneValueObjectToModel(phone));
                }
            }

            return phones;
        }

        private PhoneViewModel ConvertPhoneValueObjectToModel(Phone phone)
        {
            PhoneViewModel model = new();
            model.Id = phone.Id;
            model.PhoneNumber = $"({phone.AreaCode}) {phone.Number}";
            model.PhoneType = phone.Type;
            model.TalkTo = phone.TalkTo;
            return model;
        }
    }
}