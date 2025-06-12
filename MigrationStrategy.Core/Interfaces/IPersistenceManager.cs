namespace MigrationStrategy.Core.Interfaces
{
    using MigrationStrategy.Core.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the contract for persistence operations on products, categories, and their relationships.
    /// </summary>
    public interface IPersistenceManager
    {
        Category CreateCategory(string name, Group group);
        Product CreateProduct(string name, Group group);
        void AddProductToCategory(Product product, Category category);
        void RemoveProductFromCategory(Product product, Category category);
        void UpdateProduct(Product product, string newName);
        void UpdateCategory(Category category, string newName);
        void DeleteProduct(Product product);
        void DeleteCategory(Category category);
        IReadOnlyList<Category> GetCategories(Product product);
        IReadOnlyList<Product> GetProducts(Category category);
    }
}
