using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.StoreCommands.UpdateStore
{
    public class UpdateStoreHandler : CommandHandlerBase<UpdateStoreCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoreRepository _storeRepository;

        public UpdateStoreHandler(IUnitOfWork unitOfWork, IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
        }

        public override async Task<ICommandResult> HandleAsync(UpdateStoreCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_UPDATE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_UPDATE_COMMAND, errors);
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

                // Retrieve Entity
                Store? store = await _storeRepository.ReadAsync(command.Id.Value);

                if (store is null)
                {
                    AddNotification(nameof(command.Id), StoreCommandMessages.ERROR_STORE_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_STORE_NOT_FOUND);
                    return new CommandResult(false, StoreCommandMessages.ERROR_STORE_NOT_FOUND, errors);
                }

                store.UpdateStoreInfo(command.Name, storeAddress);
                store.SetLastUpdateDate(DateTime.UtcNow);

                // Entity validations
                AddNotifications(store, storeAddress);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_UPDATE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_UPDATE_COMMAND, errors);
                }

                // Check for repeated Address
                if (await _storeRepository.AddressExistsExceptForGivenStore(store.Address, store.Id))
                {
                    AddNotification(nameof(store.Address), StoreCommandMessages.REPEATED_STORE_ADDRESS);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_REPEATED_STORE_ADDRESS);
                    return new CommandResult(false, StoreCommandMessages.REPEATED_STORE_ADDRESS, errors);
                }

                // Persist data
                await _storeRepository.UpdateAsync(store);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_UPDATE_STORE_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
