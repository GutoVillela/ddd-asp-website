using Flunt.Notifications;
using Flunt.Validations;
using KadoshShared.Commands;
using KadoshShared.Constants.ValidationErrors;

namespace KadoshDomain.Commands.SettingsCommands.ImportDataFromLegacy
{
    public class ImportDataFromLegacyCommand : Notifiable<Notification>, ICommand
    {
        public string? Server { get; set; }

        public string? LegacyDatabaseName { get; set; }

        public int? DefaultCategoryId { get; set; }

        public int? DefaultBrandId { get; set; }

        public int? DefaultStoreId { get; set; }
        
        public int? DefaultSellerId { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNullOrEmpty(Server, nameof(Server), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_SERVER)
                .IsNotNullOrEmpty(LegacyDatabaseName, nameof(LegacyDatabaseName), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_DATABASE_NAME)
                .IsNotNull(DefaultCategoryId, nameof(DefaultCategoryId), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_DEFAULT_CATEGORY)
                .IsNotNull(DefaultBrandId, nameof(DefaultBrandId), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_DEFAULT_BRAND)
                .IsNotNull(DefaultStoreId, nameof(DefaultStoreId), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_DEFAULT_STORE)
                .IsNotNull(DefaultSellerId, nameof(DefaultSellerId), SettingsValidationsErrors.INVALID_IMPORT_LEGACY_DATA_DEFAULT_SELLER)
            );
        }
    }
}
