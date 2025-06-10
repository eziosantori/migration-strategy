using MigrationStrategy.Core.Models;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Models
{
    public class GroupTests
    {
        [Fact]
        public void GetName_ReturnsGroupName()
        {
            // Arrange
            var group = new Group("TestGroup");

            // Act
            var name = group.GetName();

            // Assert
            Assert.Equal("TestGroup", name);
        }
        [Fact]
        public void Constructor_ThrowsArgumentException_WhenNameIsNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Group(string.Empty));
        }
    }
}
