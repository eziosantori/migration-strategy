using MigrationStrategy.Core.Models;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Models
{
    public class ProductTests
    {
        [Fact]
        public void GetName_ReturnsProductName()
        {
            // Arrange
            var product = new Product("TestProduct");

            // Act
            var name = product.GetName();

            // Assert
            Assert.Equal("TestProduct", name);
        }

        [Fact]
        public void Constructor_ThrowsArgumentException_WhenNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Product(string.Empty));
        }
    }
}
