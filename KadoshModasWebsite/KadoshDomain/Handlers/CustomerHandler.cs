using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Handlers;

namespace KadoshDomain.Handlers
{
    public class CustomerHandler : Notifiable<Notification>, IHandler<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _repository;

        public CustomerHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> HandleAsync(CreateCustomerCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if(!command.IsValid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível cadastrar o cliente");
            }

            // Create Entity
            Email? email = null;
            Document? document = null;
            Address? address = null;

            if(!string.IsNullOrEmpty(command.EmailAddress))
                email = new(command.EmailAddress);

            if(!string.IsNullOrEmpty(command.DocumentNumber))
                document = new Document(command.DocumentNumber, command.DocumentType);

            if (!string.IsNullOrEmpty(command.AddressStreet))
                address = new(
                    command.AddressStreet,
                    command.AddressNumber ?? string.Empty,
                    command.AddressNeighborhood ?? string.Empty,
                    command.AddressCity ?? string.Empty,
                    command.AddressState ?? string.Empty,
                    command.AddressZipCode ?? string.Empty,
                    command.AddressComplement ?? string.Empty
                    );

            Customer customer = new(
                name: command.Name,
                email: email,
                document: document,
                gender: command.Gender,
                address: address,
                phones: command.Phones?.ToList()
                );

            // Group validations
            AddNotifications(email, document, address, customer);

            // Validate before register customer
            if (!IsValid)
                return new CommandResult(false, "Não foi possível cadastrar o cliente");

            // Create register
            await _repository.CreateAsync(customer);

            return new CommandResult(true, "Cliente cadastrado com sucesso");
        }
    }
}
