using KadoshDomain.Commands.SettingsCommands.ImportDataFromLegacy;
using KadoshShared.Commands;
using KadoshShared.Handlers;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class SettingsApplicationService : ISettingsApplicationService
    {
        private readonly ICommandHandler<ImportDataFromLegacyCommand> _importDataFromLegacyHandler;

        public SettingsApplicationService(ICommandHandler<ImportDataFromLegacyCommand> importDataFromLegacyHandler)
        {
            _importDataFromLegacyHandler = importDataFromLegacyHandler;
        }

        public async Task<ICommandResult> ImportDataFromLegacyAsync(ImportFromLegacyViewModel settingsToImport)
        {
            ImportDataFromLegacyCommand command = new();
            command.Server = settingsToImport.Server;
            command.LegacyDatabaseName = settingsToImport.LegacyDatabaseName;
            command.DefaultCategoryId = settingsToImport.DefaultCategoryId;
            command.DefaultBrandId = settingsToImport.DefaultBrandId;
            command.DefaultStoreId = settingsToImport.DefaultStoreId;
            command.DefaultSellerId = settingsToImport.DefaultSellerId;

            return await _importDataFromLegacyHandler.HandleAsync(command);
        }
    }
}