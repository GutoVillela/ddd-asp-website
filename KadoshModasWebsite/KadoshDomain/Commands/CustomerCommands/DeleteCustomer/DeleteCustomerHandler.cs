using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CustomerCommands.DeleteCustomer
{
    public class DeleteCustomerHandler : CommandHandlerBase<DeleteCustomerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;

        public DeleteCustomerHandler(IUnitOfWork unitOfWork, ICustomerRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(DeleteCustomerCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_DELETE_COMMAND, errors);
                }

                // Get Entity
                Customer? customer = await _repository.ReadAsync(command.Id ?? 0);

                // Entity validations
                if (customer is null)
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_DELETE_COMMAND, errors);
                }

                // Persist data
                _repository.Delete(customer!);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_CUSTOMER_DELETE_COMMAND);

            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
