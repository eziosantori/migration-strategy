namespace MigrationStrategy.Core.Persistence.EntityFramework.Entities
{
    /// <summary>
    /// Entity Framework representation of a Group.
    /// </summary>
    public class GroupEntity
    {
        /// <summary>
        /// Unique identifier for the group.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the group (must be unique).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Collection of products in this group.
        /// </summary>
        public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();

        /// <summary>
        /// Collection of categories in this group.
        /// </summary>
        public virtual ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
    }
}
