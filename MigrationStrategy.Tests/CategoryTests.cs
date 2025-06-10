using MigrationStrategy.Core.Models;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Models
{
    public class CategoryTests
    {
        [Fact]
        public void GetName_ReturnsCategoryName()
        {
            // Arrange
            var category = new Category("TestCategory");

            // Act
            var name = category.GetName();

            // Assert
            Assert.Equal("TestCategory", name);
        }

        [Fact]
        public void Constructor_ThrowsArgumentException_WhenNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Category(string.Empty));
        }
    }
}
