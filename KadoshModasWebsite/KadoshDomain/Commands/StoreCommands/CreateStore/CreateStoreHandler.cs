using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshDomain.ValueObjects;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.StoreCommands.CreateStore
{
    public class CreateStoreHandler : CommandHandlerBase<CreateStoreCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoreRepository _storeRepository;

        public CreateStoreHandler(IUnitOfWork unitOfWork, IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
        }

        public override async Task<ICommandResult> HandleAsync(CreateStoreCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);

                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_CREATE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_CREATE_COMMAND, errors);
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
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_CREATE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_CREATE_COMMAND, errors);
                }

                // Check for repeated Address
                if (await _storeRepository.AddressExists(store.Address))
                {
                    AddNotification(nameof(store.Address), StoreCommandMessages.REPEATED_STORE_ADDRESS);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_REPEATED_STORE_ADDRESS);
                    return new CommandResult(false, StoreCommandMessages.REPEATED_STORE_ADDRESS, errors);
                }

                // Persist data
                await _storeRepository.CreateAsync(store);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_CREATE_STORE_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
