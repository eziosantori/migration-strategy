using System;
using System.Collections.Generic;
using MigrationStrategy.Core.Interfaces;
using MigrationStrategy.Core.Models;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Interfaces
{
    public class IPersistenceManagerTests
    {
        private class DummyPersistenceManager : IPersistenceManager
        {
            public Category CreateCategory(string name, Group group) => throw new NotImplementedException();
            public Product CreateProduct(string name, Group group) => throw new NotImplementedException();
            public void AddProductToCategory(Product product, Category category) => throw new NotImplementedException();
            public void RemoveProductFromCategory(Product product, Category category) => throw new NotImplementedException();
            public void UpdateProduct(Product product, string newName) => throw new NotImplementedException();
            public void UpdateCategory(Category category, string newName) => throw new NotImplementedException();
            public void DeleteProduct(Product product) => throw new NotImplementedException();
            public void DeleteCategory(Category category) => throw new NotImplementedException();
            public IReadOnlyList<Category> GetCategories(Product product) => throw new NotImplementedException();
            public IReadOnlyList<Product> GetProducts(Category category) => throw new NotImplementedException();
        }

        [Fact]
        public void Interface_Has_All_Methods_With_Correct_Signatures()
        {
            var type = typeof(IPersistenceManager);
            Assert.NotNull(type.GetMethod("CreateCategory"));
            Assert.NotNull(type.GetMethod("CreateProduct"));
            Assert.NotNull(type.GetMethod("AddProductToCategory"));
            Assert.NotNull(type.GetMethod("RemoveProductFromCategory"));
            Assert.NotNull(type.GetMethod("UpdateProduct"));
            Assert.NotNull(type.GetMethod("UpdateCategory"));
            Assert.NotNull(type.GetMethod("DeleteProduct"));
            Assert.NotNull(type.GetMethod("DeleteCategory"));
            Assert.NotNull(type.GetMethod("GetCategories"));
            Assert.NotNull(type.GetMethod("GetProducts"));
        }
    }
}
