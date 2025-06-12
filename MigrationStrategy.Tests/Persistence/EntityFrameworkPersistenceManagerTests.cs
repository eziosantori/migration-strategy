using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using MigrationStrategy.Core.Persistence.EntityFramework;
using System;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Persistence
{
    public class EntityFrameworkPersistenceManagerTests
    {
        [Fact]
        public void CreateProduct_Should_ReturnProduct_WithCorrectName()
        {
            // Arrange - Use a unique database name for each test to avoid conflicts
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
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
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
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
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
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
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
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
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
            var group = new Group("TestGroup");
            var product = persistenceManager.CreateProduct("TestProduct", group);
            var newName = "UpdatedProduct";

            // Act
            persistenceManager.UpdateProduct(product, newName);

            // Assert
            Assert.Equal(newName, product.GetName());
        }

        [Fact]
        public void UpdateCategory_Should_ChangeCategoryName()
        {
            // Arrange
            var persistenceManager = EntityFrameworkPersistenceManagerFactory.CreateInMemory(
                $"TestDb_{Guid.NewGuid()}");
            var group = new Group("TestGroup");
            var category = persistenceManager.CreateCategory("TestCategory", group);
            var newName = "UpdatedCategory";

            // Act
            persistenceManager.UpdateCategory(category, newName);

            // Assert
            Assert.Equal(newName, category.GetName());
        }
    }
}
