using Microsoft.EntityFrameworkCore;
using MigrationStrategy.Core.Persistence.EntityFramework.Entities;

namespace MigrationStrategy.Core.Persistence.EntityFramework
{
    /// <summary>
    /// Entity Framework DbContext for the Migration Strategy application.
    /// </summary>
    public class MigrationDbContext : DbContext
    {
        /// <summary>
        /// Products in the database.
        /// </summary>
        public DbSet<ProductEntity> Products { get; set; } = null!;

        /// <summary>
        /// Categories in the database.
        /// </summary>
        public DbSet<CategoryEntity> Categories { get; set; } = null!;

        /// <summary>
        /// Groups in the database.
        /// </summary>
        public DbSet<GroupEntity> Groups { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure group
            modelBuilder.Entity<GroupEntity>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<GroupEntity>()
                .HasIndex(g => g.Name)
                .IsUnique();

            // Configure product
            modelBuilder.Entity<ProductEntity>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<ProductEntity>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure category
            modelBuilder.Entity<CategoryEntity>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<CategoryEntity>()
                .HasOne(c => c.Group)
                .WithMany(g => g.Categories)
                .HasForeignKey(c => c.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure many-to-many relationship between products and categories
            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductCategories"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
