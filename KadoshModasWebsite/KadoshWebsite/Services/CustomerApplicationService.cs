using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshDomain.Repositories;
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

namespace KadoshWebsite.Services
{
    public class CustomerApplicationService : ICustomerApplicationService
    {
        private readonly ICommandHandler<CreateCustomerCommand> _createCustomerHandler;
        private readonly ICommandHandler<DeleteCustomerCommand> _deleteCustomerHandler;
        private readonly ICommandHandler<UpdateCustomerCommand> _updateCustomerHandler;
        private readonly ICommandHandler<InformPaymentCommand> _informPaymentHandler;
        private readonly IQueryHandler<GetAllCustomersQuery, GetAllCustomersQueryResult> _getAllCustomersQueryHandler;
        private readonly IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult> _getCustomerByIdQueryHandler;
        private readonly IQueryHandler<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult> _getCustomerTotalDebtQueryHandler;

        public CustomerApplicationService(
            ICommandHandler<CreateCustomerCommand> createCustomerHandler,
            ICommandHandler<DeleteCustomerCommand> deleteCustomerHandler,
            ICommandHandler<UpdateCustomerCommand> updateCustomerHandler,
            ICommandHandler<InformPaymentCommand> informPaymentHandler,
            IQueryHandler<GetAllCustomersQuery, GetAllCustomersQueryResult> getAllCustomersQueryHandler,
            IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult> getCustomerByIdQueryHandler,
            IQueryHandler<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult> getCustomerTotalDebtQueryHandler)
        {
            _createCustomerHandler = createCustomerHandler;
            _deleteCustomerHandler = deleteCustomerHandler;
            _updateCustomerHandler = updateCustomerHandler;
            _informPaymentHandler = informPaymentHandler;
            _getAllCustomersQueryHandler = getAllCustomersQueryHandler;
            _getCustomerByIdQueryHandler = getCustomerByIdQueryHandler;
            _getCustomerTotalDebtQueryHandler = getCustomerTotalDebtQueryHandler;
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
                customerViewModels.Add(new CustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Street = customer.Street,
                    Number = customer.Number,
                    Neighborhood = customer.Neighborhood,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    Complement = customer.Complement
                });
            }
            return customerViewModels;
        }

        public async Task<CustomerViewModel> GetCustomerAsync(int id)
        {
            GetCustomerByIdQuery query = new();
            query.CustomerId = id;

            var result = await _getCustomerByIdQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            CustomerViewModel viewModel = new()
            {
                Id = result.Customer!.Id,
                Name = result.Customer!.Name,
                Gender = result.Customer!.Gender,
                Email = result.Customer!.Email,
                DocumentType = result.Customer!.DocumentType,
                DocumentNumber = result.Customer!.DocumentNumber,
                Street = result.Customer!.Street,
                Number = result.Customer!.Number,
                Neighborhood = result.Customer!.Neighborhood,
                City = result.Customer!.City,
                State = result.Customer!.State,
                ZipCode = result.Customer!.ZipCode,
                Complement = result.Customer!.Complement
            };

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

        private Phone ConvertPhoneModelToValueObject(PhoneViewModel phoneViewModel)
        {
            string fullPhoneNumber = phoneViewModel.PhoneNumber.Replace("(","").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            string areaCode = fullPhoneNumber[..2];
            string phoneNumber = fullPhoneNumber[2..];

            if(phoneViewModel.Id.HasValue)
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
    }
}