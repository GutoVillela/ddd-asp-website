using KadoshShared.Commands;
using KadoshWebsite.Models;

namespace KadoshWebsite.Services.Interfaces
{
    public interface ISettingsApplicationService
    {
        Task<ICommandResult> ImportDataFromLegacyAsync(ImportFromLegacyViewModel settingsToImport);
    }
}
