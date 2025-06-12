namespace MigrationStrategy.Core.Persistence.EntityFramework.Entities
{
    /// <summary>
    /// Entity Framework representation of a Category.
    /// </summary>
    public class CategoryEntity
    {
        /// <summary>
        /// Unique identifier for the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category (must be unique).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Reference to the group this category belongs to.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Navigation property to the group this category belongs to.
        /// </summary>
        public virtual GroupEntity? Group { get; set; }

        /// <summary>
        /// Collection of products in this category.
        /// </summary>
        public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
