namespace MigrationStrategy.Core.Services
{
    using MigrationStrategy.Core.Interfaces;
    using MigrationStrategy.Core.Models;
    using System;

    /// <summary>
    /// Service responsible for migrating products and categories between groups.
    /// </summary>
    public class MigrationService
    {
        private readonly IPersistenceManager _persistenceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationService"/> class.
        /// </summary>
        /// <param name="persistenceManager">The persistence manager to use for data operations.</param>
        /// <exception cref="ArgumentNullException">Thrown if persistenceManager is null.</exception>
        public MigrationService(IPersistenceManager persistenceManager)
        {
            _persistenceManager = persistenceManager ?? throw new ArgumentNullException(nameof(persistenceManager));
        }

        /// <summary>
        /// Migrates a product or category to the specified group using the specified migration type.
        /// </summary>
        /// <param name="objectToMigrate">The product or category to migrate.</param>
        /// <param name="targetGroup">The group to migrate to.</param>
        /// <param name="migrationType">The type of migration to perform.</param>
        /// <exception cref="ArgumentNullException">Thrown if objectToMigrate or targetGroup is null.</exception>
        /// <exception cref="ArgumentException">Thrown if objectToMigrate is not a Product or Category.</exception>
        public void Migrate(IMoveItem objectToMigrate, Group targetGroup, MigrationType migrationType)
        {
            if (objectToMigrate == null)
            {
                throw new ArgumentNullException(nameof(objectToMigrate));
            }

            if (targetGroup == null)
            {
                throw new ArgumentNullException(nameof(targetGroup));
            }

            if (!(objectToMigrate is Product) && !(objectToMigrate is Category))
            {
                throw new ArgumentException($"Object to migrate must be either a Product or a Category, but was {objectToMigrate.GetType().Name}");
            }

            if (migrationType == MigrationType.COPY_SINGLE)
            {
                if (objectToMigrate is Product product)
                {
                    string baseName = product.GetName();
                    string newName = baseName;
                    int attempt = 0;
                    while (true)
                    {
                        try
                        {
                            _persistenceManager.CreateProduct(newName, targetGroup);
                            break;
                        }
                        catch (InvalidOperationException)
                        {
                            attempt++;
                            newName = attempt == 1 ? $"{baseName}-copy" : $"{baseName}-copy{attempt}";
                        }
                    }
                }
                else if (objectToMigrate is Category category)
                {
                    string baseName = category.GetName();
                    string newName = baseName;
                    int attempt = 0;
                    while (true)
                    {
                        try
                        {
                            _persistenceManager.CreateCategory(newName, targetGroup);
                            break;
                        }
                        catch (InvalidOperationException)
                        {
                            attempt++;
                            newName = attempt == 1 ? $"{baseName}-copy" : $"{baseName}-copy{attempt}";
                        }
                    }
                }
            }
        }
    }
}