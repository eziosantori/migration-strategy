namespace MigrationStrategy.Core.Persistence.EntityFramework.Entities
{
    /// <summary>
    /// Entity Framework representation of a Product.
    /// </summary>
    public class ProductEntity
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the product (must be unique).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Reference to the group this product belongs to.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Navigation property to the group this product belongs to.
        /// </summary>
        public virtual GroupEntity? Group { get; set; }

        /// <summary>
        /// Collection of categories this product belongs to.
        /// </summary>
        public virtual ICollection<CategoryEntity> Categories { get; set; } = new List<CategoryEntity>();
    }
}
