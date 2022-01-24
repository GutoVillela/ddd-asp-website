using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Handlers;

namespace KadoshDomain.Handlers
{
    public class StoreHandler : Notifiable<Notification>, IHandler<CreateStoreCommand>
    {

        private readonly IStoreRepository _storeRepository;

        public StoreHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<ICommandResult> HandleAsync(CreateStoreCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_CREATE_COMMAND);
            }

            // Create Value Objects
            Address storeAddress = new(
                command.AddressStreet,
                command.AddressNumber,
                command.AddressNeighborhood,
                command.AddressCity, 
                command.AddressState, 
                command.AddressZipCode,
                command.AddressComplement);

            // Create Entity
            Store store = new(command.Name, storeAddress);

            // Entity validations
            AddNotifications(store, storeAddress);

            // Check validations
            if(!IsValid)
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_CREATE_COMMAND);

            // Persist data
            await _storeRepository.CreateAsync(store);

            return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_CREATE_STORE_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(UpdateStoreCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_UPDATE_COMMAND);
            }

            // Create Value Objects
            Address storeAddress = new(
                command.AddressStreet,
                command.AddressNumber,
                command.AddressNeighborhood,
                command.AddressCity,
                command.AddressState,
                command.AddressZipCode,
                command.AddressComplement);

            // Create Entity
            Store store = new(command.Id.Value, command.Name, storeAddress);
            store.SetLastUpdateDate(DateTime.UtcNow);

            // Entity validations
            AddNotifications(store, storeAddress);

            // Check validations
            if (!IsValid)
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_UPDATE_COMMAND);

            // Persist data
            await _storeRepository.UpdateAsync(store);

            return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_UPDATE_STORE_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(DeleteStoreCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_DELETE_COMMAND);
            }

            // Get Entity
            Store store = await _storeRepository.ReadAsync(command.Id ?? 0);

            // Entity validations
            if (store is null)
                AddNotification(nameof(store), StoreCommandMessages.COULD_NOT_FIND_STORE);

            // Check validations
            if (!IsValid)
                return new CommandResult(false, StoreCommandMessages.INVALID_STORE_DELETE_COMMAND);

            // Persist data
            await _storeRepository.DeleteAsync(store);

            return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_DELETE_STORE_COMMAND);
        }
    }
}
