using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.StoreCommands.DeleteStore
{
    public class DeleteStoreHandler : CommandHandlerBase<DeleteStoreCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoreRepository _storeRepository;

        public DeleteStoreHandler(IUnitOfWork unitOfWork, IStoreRepository storeRepository)
        {
            _unitOfWork = unitOfWork;
            _storeRepository = storeRepository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteStoreCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_DELETE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_DELETE_COMMAND, errors);
                }

                // Get Entity
                Store? store = await _storeRepository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (store is null)
                    AddNotification(nameof(store), StoreCommandMessages.COULD_NOT_FIND_STORE);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_STORE_DELETE_COMMAND);
                    return new CommandResult(false, StoreCommandMessages.INVALID_STORE_DELETE_COMMAND, errors);
                }

                // Persist data
                _storeRepository.Delete(store!);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, StoreCommandMessages.SUCCESS_ON_DELETE_STORE_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
