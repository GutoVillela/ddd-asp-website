using KadoshShared.Entities;

namespace KadoshDomain.Commands.SettingsCommands.ImportDataFromLegacy
{
    /// <summary>
    /// Class that defines the legacy and the imported from legacy classes.
    /// </summary>
    /// <typeparam name="TEntity">Entity class.</typeparam>
    /// <typeparam name="TLegacyEntity">Legacy Entity class related to the domain Entity class.</typeparam>
    internal class ImportFromLegacyMap<TEntity, TLegacyEntity> where TEntity : Entity where TLegacyEntity : LegacyEntity<TEntity>
    {
        public ImportFromLegacyMap(TLegacyEntity legacyEntity, TEntity importedEntity)
        {
            LegacyEntity = legacyEntity;
            ImportedEntity = importedEntity;
        }

        public TLegacyEntity LegacyEntity { get; set; }
        public TEntity ImportedEntity { get; set; }
    }
}
