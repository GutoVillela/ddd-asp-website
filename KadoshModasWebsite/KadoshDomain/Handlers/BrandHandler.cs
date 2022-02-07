﻿
using Flunt.Notifications;
using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Handlers;
using KadoshShared.ValueObjects;

namespace KadoshDomain.Handlers
{
    public class BrandHandler : Notifiable<Notification>, IHandler<CreateBrandCommand>, IHandler<UpdateBrandCommand>, IHandler<DeleteBrandCommand>
    {
        private readonly IBrandRepository _repository;

        public BrandHandler(IBrandRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICommandResult> HandleAsync(CreateBrandCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_CREATE_COMMAND, errors);

            }

            // Create Entity
            Brand brand = new(
                name: command.Name
                );

            // Group validations
            AddNotifications(brand);

            // Validate before register customer
            if (!IsValid)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_CREATE_COMMAND, errors);
            }

            // Create register
            await _repository.CreateAsync(brand);

            return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_CREATE_BRAND_COMMAND);
        }
        public async Task<ICommandResult> HandleAsync(UpdateBrandCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_UPDATE_COMMAND, errors);

            }

            // Recover entity to update its values
            Brand brand = await _repository.ReadAsync(command.Id.Value);

            if (brand is null)
            {
                AddNotification(nameof(command.Id), BrandCommandMessages.ERROR_BRAND_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_BRAND_NOT_FOUND);
                return new CommandResult(false, BrandCommandMessages.ERROR_BRAND_NOT_FOUND, errors);
            }

            brand.UpdateBrandInfo(
                name: command.Name
                );

            brand.SetLastUpdateDate(DateTime.UtcNow);

            // Group validations
            AddNotifications(brand);

            // Validate before register customer
            if (!IsValid)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_UPDATE_COMMAND, errors);
            }

            // Update entity
            await _repository.UpdateAsync(brand);

            return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_UPDATE_BRAND_COMMAND);
        }

        public async Task<ICommandResult> HandleAsync(DeleteBrandCommand command)
        {
            // Fail Fast Validation
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_DELETE_COMMAND, errors);
            }

            // Get Entity
            Brand brand = await _repository.ReadAsync(command.Id ?? 0);

            // Entity validations
            if (brand is null)
                AddNotification(nameof(brand), BrandCommandMessages.ERROR_BRAND_NOT_FOUND);

            // Check validations
            if (!IsValid)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND);
                return new CommandResult(false, BrandCommandMessages.INVALID_BRAND_DELETE_COMMAND, errors);
            }

            // Delete data
            await _repository.DeleteAsync(brand);

            return new CommandResult(true, BrandCommandMessages.SUCCESS_ON_DELETE_BRAND_COMMAND);
        }

        private ICollection<Error> GetErrorsFromNotifications(int errorCode)
        {
            HashSet<Error> errors = new();
            foreach (var error in Notifications)
            {
                errors.Add(new Error(errorCode, error.Message));
            }

            return errors;
        }

    }
}