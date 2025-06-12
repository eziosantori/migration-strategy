using Microsoft.EntityFrameworkCore;
using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using MigrationStrategy.Core.Persistence.EntityFramework;
using MigrationStrategy.Core.Persistence.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MigrationStrategy.Core.Persistence
{
    /// <summary>
    /// Entity Framework implementation of the IPersistenceManager interface.
    /// </summary>
    public class EntityFrameworkPersistenceManager : IPersistenceManager
    {
        private readonly MigrationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkPersistenceManager"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EntityFrameworkPersistenceManager(MigrationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a new product and associates it with the specified group.
        /// </summary>
        /// <param name="name">The name of the product to create.</param>
        /// <param name="group">The group to associate with the product.</param>
        /// <returns>The created Product object.</returns>
        public Product CreateProduct(string name, Group group)
        {
            // Check if a product with the given name already exists
            if (_context.Products.Any(p => p.Name == name))
            {
                throw new InvalidOperationException($"A product with name '{name}' already exists.");
            }

            // Get or create the group entity
            var groupEntity = GetOrCreateGroupEntity(group);

            // Create new product entity
            var productEntity = new ProductEntity
            {
                Name = name,
                GroupId = groupEntity.Id,
                Group = groupEntity
            };

            _context.Products.Add(productEntity);
            _context.SaveChanges();

            // Return domain model
            return new Product(name);
        }

        /// <summary>
        /// Creates a new category and associates it with the specified group.
        /// </summary>
        /// <param name="name">The name of the category to create.</param>
        /// <param name="group">The group to associate with the category.</param>
        /// <returns>The created Category object.</returns>
        public Category CreateCategory(string name, Group group)
        {
            // Check if a category with the given name already exists
            if (_context.Categories.Any(c => c.Name == name))
            {
                throw new InvalidOperationException($"A category with name '{name}' already exists.");
            }

            // Get or create the group entity
            var groupEntity = GetOrCreateGroupEntity(group);

            // Create new category entity
            var categoryEntity = new CategoryEntity
            {
                Name = name,
                GroupId = groupEntity.Id,
                Group = groupEntity
            };

            _context.Categories.Add(categoryEntity);
            _context.SaveChanges();

            // Return domain model
            return new Category(name);
        }

        /// <summary>
        /// Adds a product to a category, creating the relationship between them.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="category">The category to which the product should be added.</param>
        public void AddProductToCategory(Product product, Category category)
        {
            var productName = product.GetName();
            var categoryName = category.GetName();

            var productEntity = _context.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Name == productName);

            var categoryEntity = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Name == categoryName);

            if (productEntity == null)
            {
                throw new InvalidOperationException($"Product '{productName}' not found.");
            }

            if (categoryEntity == null)
            {
                throw new InvalidOperationException($"Category '{categoryName}' not found.");
            }

            // Add the relationship if it doesn't exist
            if (!productEntity.Categories.Contains(categoryEntity))
            {
                productEntity.Categories.Add(categoryEntity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a product from a category, breaking the relationship between them.
        /// </summary>
        /// <param name="product">The product to remove.</param>
        /// <param name="category">The category from which the product should be removed.</param>
        public void RemoveProductFromCategory(Product product, Category category)
        {
            var productName = product.GetName();
            var categoryName = category.GetName();

            var productEntity = _context.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Name == productName);

            var categoryEntity = _context.Categories
                .FirstOrDefault(c => c.Name == categoryName);

            if (productEntity == null || categoryEntity == null)
            {
                return; // Nothing to remove
            }

            var categoryToRemove = productEntity.Categories.FirstOrDefault(c => c.Name == categoryName);
            if (categoryToRemove != null)
            {
                productEntity.Categories.Remove(categoryToRemove);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a product's name.
        /// </summary>
        /// <param name="product">The product to update.</param>
        /// <param name="newName">The new name for the product.</param>
        public void UpdateProduct(Product product, string newName)
        {
            var oldName = product.GetName();

            if (oldName == newName)
            {
                return; // No change needed
            }

            // Check if a product with the new name already exists
            if (_context.Products.Any(p => p.Name == newName))
            {
                throw new InvalidOperationException($"A product with name '{newName}' already exists.");
            }

            var productEntity = _context.Products.FirstOrDefault(p => p.Name == oldName);
            if (productEntity == null)
            {
                throw new InvalidOperationException($"Product '{oldName}' not found.");
            }

            // Update the entity name
            productEntity.Name = newName;
            _context.SaveChanges();

            // Update the domain model
            typeof(Product).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(product, newName);
        }

        /// <summary>
        /// Updates a category's name.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <param name="newName">The new name for the category.</param>
        public void UpdateCategory(Category category, string newName)
        {
            var oldName = category.GetName();

            if (oldName == newName)
            {
                return; // No change needed
            }

            // Check if a category with the new name already exists
            if (_context.Categories.Any(c => c.Name == newName))
            {
                throw new InvalidOperationException($"A category with name '{newName}' already exists.");
            }

            var categoryEntity = _context.Categories.FirstOrDefault(c => c.Name == oldName);
            if (categoryEntity == null)
            {
                throw new InvalidOperationException($"Category '{oldName}' not found.");
            }

            // Update the entity name
            categoryEntity.Name = newName;
            _context.SaveChanges();

            // Update the domain model
            typeof(Category).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(category, newName);
        }

        /// <summary>
        /// Deletes a product from the system.
        /// </summary>
        /// <param name="product">The product to delete.</param>
        public void DeleteProduct(Product product)
        {
            var productName = product.GetName();
            var productEntity = _context.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Name == productName);

            if (productEntity == null)
            {
                return; // Nothing to delete
            }

            // Remove relationships to categories
            productEntity.Categories.Clear();

            // Delete the product
            _context.Products.Remove(productEntity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a category from the system.
        /// </summary>
        /// <param name="category">The category to delete.</param>
        public void DeleteCategory(Category category)
        {
            var categoryName = category.GetName();
            var categoryEntity = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Name == categoryName);

            if (categoryEntity == null)
            {
                return; // Nothing to delete
            }

            // Remove relationships to products
            categoryEntity.Products.Clear();

            // Delete the category
            _context.Categories.Remove(categoryEntity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets all categories to which a product belongs.
        /// </summary>
        /// <param name="product">The product whose categories should be retrieved.</param>
        /// <returns>A list of categories.</returns>
        public IReadOnlyList<Category> GetCategories(Product product)
        {
            var productName = product.GetName();
            var productEntity = _context.Products
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.Name == productName);

            if (productEntity == null)
            {
                return new List<Category>().AsReadOnly();
            }

            var categories = productEntity.Categories
                .Select(c => new Category(c.Name))
                .ToList();

            return categories.AsReadOnly();
        }

        /// <summary>
        /// Gets all products that belong to a category.
        /// </summary>
        /// <param name="category">The category whose products should be retrieved.</param>
        /// <returns>A list of products.</returns>
        public IReadOnlyList<Product> GetProducts(Category category)
        {
            var categoryName = category.GetName();
            var categoryEntity = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Name == categoryName);

            if (categoryEntity == null)
            {
                return new List<Product>().AsReadOnly();
            }

            var products = categoryEntity.Products
                .Select(p => new Product(p.Name))
                .ToList();

            return products.AsReadOnly();
        }

        /// <summary>
        /// Gets or creates a group entity from a domain Group object.
        /// </summary>
        /// <param name="group">The domain Group object.</param>
        /// <returns>The corresponding GroupEntity from the database.</returns>
        private GroupEntity GetOrCreateGroupEntity(Group group)
        {
            var groupName = group.GetName();
            var groupEntity = _context.Groups.FirstOrDefault(g => g.Name == groupName);

            if (groupEntity == null)
            {
                groupEntity = new GroupEntity { Name = groupName };
                _context.Groups.Add(groupEntity);
                _context.SaveChanges();
            }

            return groupEntity;
        }
    }
}
