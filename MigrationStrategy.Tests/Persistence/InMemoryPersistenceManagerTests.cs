using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using MigrationStrategy.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Persistence
{
    public class InMemoryPersistenceManagerTests
    {
        [Fact]
        public void CreateProduct_Should_ReturnProduct_WithCorrectName()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            string productName = "TestProduct";

            // Act
            var product = persistenceManager.CreateProduct(productName, group);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(productName, product.GetName());
        }

        [Fact]
        public void CreateCategory_Should_ReturnCategory_WithCorrectName()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            string categoryName = "TestCategory";

            // Act
            var category = persistenceManager.CreateCategory(categoryName, group);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(categoryName, category.GetName());
        }

        [Fact]
        public void AddProductToCategory_Should_AssociateProductWithCategory()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);

            // Act
            persistenceManager.AddProductToCategory(product, category);

            // Assert
            var products = persistenceManager.GetProducts(category);
            var categories = persistenceManager.GetCategories(product);
            Assert.Single(products);
            Assert.Single(categories);
            Assert.Equal("TestProduct", products[0].GetName());
            Assert.Equal("TestCategory", categories[0].GetName());
        }

        [Fact]
        public void RemoveProductFromCategory_Should_BreakAssociation()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);

            // Act
            persistenceManager.RemoveProductFromCategory(product, category);

            // Assert
            var products = persistenceManager.GetProducts(category);
            var categories = persistenceManager.GetCategories(product);
            Assert.Empty(products);
            Assert.Empty(categories);
        }

        [Fact]
        public void UpdateProduct_Should_ChangeProductName()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var newName = "UpdatedProduct";

            // Act
            persistenceManager.UpdateProduct(product, newName);

            // Assert
            Assert.Equal(newName, product.GetName());

            // Verify that category relationships are maintained
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);
            var categories = persistenceManager.GetCategories(product);
            Assert.Single(categories);
            Assert.Equal("TestCategory", categories[0].GetName());
        }

        [Fact]
        public void UpdateProduct_Should_MaintainCategoryRelationships()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);
            var newName = "UpdatedProduct";

            // Act
            persistenceManager.UpdateProduct(product, newName);

            // Assert
            var products = persistenceManager.GetProducts(category);
            var categories = persistenceManager.GetCategories(product);

            Assert.Single(products);
            Assert.Single(categories);
            Assert.Equal(newName, products[0].GetName());
            Assert.Equal("TestCategory", categories[0].GetName());
        }

        [Fact]
        public void UpdateProduct_Should_ThrowException_WhenNameAlreadyExists()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product1 = persistenceManager.CreateProduct("Product1", group);
            var product2 = persistenceManager.CreateProduct("Product2", group);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                persistenceManager.UpdateProduct(product1, "Product2"));
            Assert.Contains("already exists", exception.Message);
        }

        [Fact]
        public void UpdateCategory_Should_ChangeCategoryName()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var category = persistenceManager.CreateCategory("TestCategory", group);
            var newName = "UpdatedCategory";

            // Act
            persistenceManager.UpdateCategory(category, newName);

            // Assert
            Assert.Equal(newName, category.GetName());
        }

        [Fact]
        public void UpdateCategory_Should_MaintainProductRelationships()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);
            var newName = "UpdatedCategory";

            // Act
            persistenceManager.UpdateCategory(category, newName);

            // Assert
            var products = persistenceManager.GetProducts(category);
            var categories = persistenceManager.GetCategories(product);

            Assert.Single(products);
            Assert.Single(categories);
            Assert.Equal("TestProduct", products[0].GetName());
            Assert.Equal(newName, categories[0].GetName());
        }

        [Fact]
        public void UpdateCategory_Should_ThrowException_WhenNameAlreadyExists()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var category1 = persistenceManager.CreateCategory("Category1", group);
            var category2 = persistenceManager.CreateCategory("Category2", group);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                persistenceManager.UpdateCategory(category1, "Category2"));
            Assert.Contains("already exists", exception.Message);
        }

        [Fact]
        public void DeleteProduct_Should_RemoveProduct()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);

            // Act
            persistenceManager.DeleteProduct(product);

            // Assert
            // Product should no longer be associated with any category
            var products = persistenceManager.GetProducts(category);
            Assert.Empty(products);
        }

        [Fact]
        public void DeleteCategory_Should_RemoveCategory()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);
            persistenceManager.AddProductToCategory(product, category);

            // Act
            persistenceManager.DeleteCategory(category);

            // Assert
            // Category should no longer be associated with any product
            var categories = persistenceManager.GetCategories(product);
            Assert.Empty(categories);
        }

        [Fact]
        public void DeleteProduct_Should_RemoveAllCategoryAssociations()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var category1 = persistenceManager.CreateCategory("TestCategory1", group);
            var category2 = persistenceManager.CreateCategory("TestCategory2", group);

            persistenceManager.AddProductToCategory(product, category1);
            persistenceManager.AddProductToCategory(product, category2);

            // Act
            persistenceManager.DeleteProduct(product);

            // Assert
            var products1 = persistenceManager.GetProducts(category1);
            var products2 = persistenceManager.GetProducts(category2);

            Assert.Empty(products1);
            Assert.Empty(products2);
        }

        [Fact]
        public void DeleteCategory_Should_RemoveAllProductAssociations()
        {
            // Arrange
            var persistenceManager = new InMemoryPersistenceManager();
            var group = new Group("TestGroup");
            var product1 = persistenceManager.CreateProduct("TestProduct1", group);
            var product2 = persistenceManager.CreateProduct("TestProduct2", group);
            var category = persistenceManager.CreateCategory("TestCategory", group);

            persistenceManager.AddProductToCategory(product1, category);
            persistenceManager.AddProductToCategory(product2, category);

            // Act
            persistenceManager.DeleteCategory(category);

            // Assert
            var categories1 = persistenceManager.GetCategories(product1);
            var categories2 = persistenceManager.GetCategories(product2);

            Assert.Empty(categories1);
            Assert.Empty(categories2);
        }
    }
}
