namespace KadoshShared.Entities
{
    public abstract class LegacyEntity<TEntity> where TEntity : Entity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
