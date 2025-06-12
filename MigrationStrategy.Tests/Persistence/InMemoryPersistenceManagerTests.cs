using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using MigrationStrategy.Core.Persistence;
using System;
using System.Collections.Generic;
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
    }
}
