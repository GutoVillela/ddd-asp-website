using KadoshDomain.Commands;
using KadoshShared.Commands;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using KadoshShared.Constants.ServicesMessages;
using KadoshDomain.Entities;
using KadoshShared.Repositories;
using KadoshDomain.Repositories;
using KadoshDomain.Handlers;

namespace KadoshWebsite.Services
{
    public class CustomerApplicationService : ICustomerApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;

        public CustomerApplicationService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
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

            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCustomerAsync(int id)
        {
            DeleteCustomerCommand command = new();
            command.Id = id;

            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.ReadAllAsync();
            var customerViewModels = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                customerViewModels.Add(new CustomerViewModel()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Street = customer.Address?.Street,
                    Number = customer.Address?.Number,
                    Neighborhood = customer.Address?.Neighborhood,
                    City = customer.Address?.City,
                    State = customer.Address?.State,
                    ZipCode = customer.Address?.ZipCode,
                    Complement = customer.Address?.Complement
                });
            }
            return customerViewModels;
        }

        public async Task<CustomerViewModel> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.ReadAsync(id);
            if (customer == null)
                throw new ApplicationException(CustomerServiceMessages.ERROR_CUSTOMER_ID_NOT_FOUND);

            CustomerViewModel viewModel = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Gender = customer.Gender,
                Email = customer.Email?.EmailAddress,
                DocumentType = customer.Document?.Type,
                DocumentNumber = customer.Document?.Number,
                Street = customer.Address?.Street,
                Number = customer.Address?.Number,
                Neighborhood = customer.Address?.Neighborhood,
                City = customer.Address?.City,
                State = customer.Address?.State,
                ZipCode = customer.Address?.ZipCode,
                Complement = customer.Address?.Complement
            };

            viewModel.Phones = GetPhonesFromEntityAsViewModels(customer);

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

            CustomerHandler customerHandler = new(_unitOfWork, _customerRepository);
            return await customerHandler.HandleAsync(command);
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

        private IEnumerable<PhoneViewModel> GetPhonesFromEntityAsViewModels(Customer customer)
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