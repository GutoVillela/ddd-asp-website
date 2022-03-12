﻿using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.UserCommands.UpdateUser
{
    public class UpdateUserHandler : HandlerBase<UpdateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _repository;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IUserRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public override async Task<ICommandResult> HandleAsync(UpdateUserCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_UPDATE_COMMAND, errors);

                }

                // Verify if username is already taken
                var usernameTaken = await _repository.VerifyIfUsernameIsTakenExceptForGivenOneAsync(command.NewUsername, command.OriginalUsername);

                if (usernameTaken)
                {
                    AddNotification(nameof(command.NewUsername), UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN);
                    return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_ALREADY_TAKEN, errors);
                }

                // Get Entity to Update
                User? user = await _repository.ReadAsync(command.Id.Value);

                if (user is null)
                {
                    AddNotification(nameof(command.OriginalUsername), UserCommandMessages.ERROR_USERNAME_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_USERNAME_NOT_FOUND);
                    return new CommandResult(false, UserCommandMessages.ERROR_USERNAME_NOT_FOUND, errors);
                }

                // Get password encrypted
                (string passwordHash, byte[] salt, int iterations) = _repository.GetPasswordHashed(command.Password);

                user.UpdateUserInfo(
                    name: command.Name,
                    username: command.NewUsername,
                    passwordHash: passwordHash,
                    passwordSalt: salt,
                    passwordSaltIterations: iterations,
                    role: command.Role.Value,
                    storeId: command.StoreId.Value
                    );

                // Group validations
                AddNotifications(user);

                // Validate before register customer
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND);
                    return new CommandResult(false, UserCommandMessages.INVALID_USER_UPDATE_COMMAND, errors);
                }

                // Create user
                await _repository.UpdateAsync(user);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, UserCommandMessages.SUCCESS_ON_UPDATE_USER_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }
        }
    }
}
