namespace MigrationStrategy.Core.Persistence
{
    using MigrationStrategy.Core.Interfaces;
    using MigrationStrategy.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// In-memory implementation of the IPersistenceManager interface for testing and development purposes.
    /// </summary>
    public class InMemoryPersistenceManager : IPersistenceManager
    {
        private readonly Dictionary<string, Product> _products = new();
        private readonly Dictionary<string, Category> _categories = new();
        private readonly Dictionary<string, Group> _groups = new();
        private readonly Dictionary<string, HashSet<string>> _productCategories = new();
        private readonly Dictionary<string, HashSet<string>> _categoryProducts = new();
        private readonly Dictionary<string, string> _productGroups = new();
        private readonly Dictionary<string, string> _categoryGroups = new();

        /// <summary>
        /// Creates a new product and associates it with the specified group.
        /// </summary>
        /// <param name="name">The name of the product to create.</param>
        /// <param name="group">The group to associate with the product.</param>
        /// <returns>The created Product object.</returns>
        public Product CreateProduct(string name, Group group)
        {
            var product = new Product(name);
            _products[name] = product;

            // Store group association
            _productGroups[name] = group.GetName();

            // Add group if not exists
            if (!_groups.ContainsKey(group.GetName()))
            {
                _groups[group.GetName()] = group;
            }

            return product;
        }

        /// <summary>
        /// Creates a new category and associates it with the specified group.
        /// </summary>
        /// <param name="name">The name of the category to create.</param>
        /// <param name="group">The group to associate with the category.</param>
        /// <returns>The created Category object.</returns>
        public Category CreateCategory(string name, Group group)
        {
            var category = new Category(name);
            _categories[name] = category;

            // Store group association
            _categoryGroups[name] = group.GetName();

            // Add group if not exists
            if (!_groups.ContainsKey(group.GetName()))
            {
                _groups[group.GetName()] = group;
            }

            return category;
        }

        /// <summary>
        /// Adds a product to a category, creating the relationship between them.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <param name="category">The category to which the product should be added.</param>
        public void AddProductToCategory(Product product, Category category)
        {
            string productName = product.GetName();
            string categoryName = category.GetName();

            // Initialize collections if they don't exist
            if (!_productCategories.ContainsKey(productName))
            {
                _productCategories[productName] = new HashSet<string>();
            }

            if (!_categoryProducts.ContainsKey(categoryName))
            {
                _categoryProducts[categoryName] = new HashSet<string>();
            }

            // Create bidirectional relationship
            _productCategories[productName].Add(categoryName);
            _categoryProducts[categoryName].Add(productName);
        }        /// <summary>
                 /// Removes a product from a category, breaking the relationship between them.
                 /// </summary>
                 /// <param name="product">The product to remove.</param>
                 /// <param name="category">The category from which the product should be removed.</param>
        public void RemoveProductFromCategory(Product product, Category category)
        {
            string productName = product.GetName();
            string categoryName = category.GetName();

            // Remove the relationship if it exists
            if (_productCategories.ContainsKey(productName))
            {
                _productCategories[productName].Remove(categoryName);
            }

            if (_categoryProducts.ContainsKey(categoryName))
            {
                _categoryProducts[categoryName].Remove(productName);
            }
        }        /// <summary>
                 /// Updates a product's name.
                 /// </summary>
                 /// <param name="product">The product to update.</param>
                 /// <param name="newName">The new name for the product.</param>
        public void UpdateProduct(Product product, string newName)
        {
            string oldName = product.GetName();

            if (oldName == newName)
            {
                return; // No change needed
            }

            if (_products.ContainsKey(newName))
            {
                throw new InvalidOperationException($"A product with name '{newName}' already exists.");
            }

            // Update the product reference in the dictionary
            _products.Remove(oldName);

            // Using reflection to change the name directly on the existing product object
            typeof(Product).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(product, newName);

            _products[newName] = product;

            // Update group association if exists
            if (_productGroups.ContainsKey(oldName))
            {
                string groupName = _productGroups[oldName];
                _productGroups.Remove(oldName);
                _productGroups[newName] = groupName;
            }

            // Update category associations if exist
            if (_productCategories.ContainsKey(oldName))
            {
                // Store the categories this product belongs to
                var categories = new HashSet<string>(_productCategories[oldName]);
                _productCategories.Remove(oldName);
                _productCategories[newName] = new HashSet<string>(categories);

                // Update the reverse relations in categoryProducts
                foreach (var categoryName in categories)
                {
                    if (_categoryProducts.ContainsKey(categoryName))
                    {
                        _categoryProducts[categoryName].Remove(oldName);
                        _categoryProducts[categoryName].Add(newName);
                    }
                }
            }
        }        /// <summary>
                 /// Updates a category's name.
                 /// </summary>
                 /// <param name="category">The category to update.</param>
                 /// <param name="newName">The new name for the category.</param>
        public void UpdateCategory(Category category, string newName)
        {
            string oldName = category.GetName();

            if (oldName == newName)
            {
                return; // No change needed
            }

            if (_categories.ContainsKey(newName))
            {
                throw new InvalidOperationException($"A category with name '{newName}' already exists.");
            }

            // Update the category reference in the dictionary
            _categories.Remove(oldName);

            // Using reflection to change the name directly on the existing category object
            typeof(Category).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(category, newName);

            _categories[newName] = category;

            // Update group association if exists
            if (_categoryGroups.ContainsKey(oldName))
            {
                string groupName = _categoryGroups[oldName];
                _categoryGroups.Remove(oldName);
                _categoryGroups[newName] = groupName;
            }

            // Update product associations if exist
            if (_categoryProducts.ContainsKey(oldName))
            {
                // Store the products in this category
                var products = new HashSet<string>(_categoryProducts[oldName]);
                _categoryProducts.Remove(oldName);
                _categoryProducts[newName] = new HashSet<string>(products);

                // Update the reverse relations in productCategories
                foreach (var productName in products)
                {
                    if (_productCategories.ContainsKey(productName))
                    {
                        _productCategories[productName].Remove(oldName);
                        _productCategories[productName].Add(newName);
                    }
                }
            }
        }        /// <summary>
                 /// Deletes a product from the system.
                 /// </summary>
                 /// <param name="product">The product to delete.</param>
        public void DeleteProduct(Product product)
        {
            string productName = product.GetName();

            // Remove from products dictionary
            if (_products.ContainsKey(productName))
            {
                _products.Remove(productName);
            }

            // Remove from group association
            if (_productGroups.ContainsKey(productName))
            {
                _productGroups.Remove(productName);
            }

            // Remove all category associations
            if (_productCategories.ContainsKey(productName))
            {
                // Get all the categories this product belongs to
                var categories = new HashSet<string>(_productCategories[productName]);

                // Remove product from all its categories
                foreach (var categoryName in categories)
                {
                    if (_categoryProducts.ContainsKey(categoryName))
                    {
                        _categoryProducts[categoryName].Remove(productName);
                    }
                }

                // Remove the product's category associations
                _productCategories.Remove(productName);
            }
        }

        /// <summary>
        /// Deletes a category from the system.
        /// </summary>
        /// <param name="category">The category to delete.</param>
        public void DeleteCategory(Category category)
        {
            string categoryName = category.GetName();

            // Remove from categories dictionary
            if (_categories.ContainsKey(categoryName))
            {
                _categories.Remove(categoryName);
            }

            // Remove from group association
            if (_categoryGroups.ContainsKey(categoryName))
            {
                _categoryGroups.Remove(categoryName);
            }

            // Remove all product associations
            if (_categoryProducts.ContainsKey(categoryName))
            {
                // Get all products in this category
                var products = new HashSet<string>(_categoryProducts[categoryName]);

                // Remove category from all its products
                foreach (var productName in products)
                {
                    if (_productCategories.ContainsKey(productName))
                    {
                        _productCategories[productName].Remove(categoryName);
                    }
                }

                // Remove the category's product associations
                _categoryProducts.Remove(categoryName);
            }
        }

        /// <summary>
        /// Gets all categories to which a product belongs.
        /// </summary>
        /// <param name="product">The product whose categories should be retrieved.</param>
        /// <returns>A list of categories.</returns>
        public IReadOnlyList<Category> GetCategories(Product product)
        {
            string productName = product.GetName();
            if (!_productCategories.ContainsKey(productName))
            {
                return new List<Category>().AsReadOnly();
            }

            var categories = new List<Category>();
            foreach (var categoryName in _productCategories[productName])
            {
                if (_categories.ContainsKey(categoryName))
                {
                    categories.Add(_categories[categoryName]);
                }
            }

            return categories.AsReadOnly();
        }

        /// <summary>
        /// Gets all products that belong to a category.
        /// </summary>
        /// <param name="category">The category whose products should be retrieved.</param>
        /// <returns>A list of products.</returns>
        public IReadOnlyList<Product> GetProducts(Category category)
        {
            string categoryName = category.GetName();
            if (!_categoryProducts.ContainsKey(categoryName))
            {
                return new List<Product>().AsReadOnly();
            }

            var products = new List<Product>();
            foreach (var productName in _categoryProducts[categoryName])
            {
                if (_products.ContainsKey(productName))
                {
                    products.Add(_products[productName]);
                }
            }

            return products.AsReadOnly();
        }
    }
}
