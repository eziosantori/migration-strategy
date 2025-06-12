using MigrationStrategy.Core.Models;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Enums
{
    public class MigrationTypeTests
    {
        [Theory]
        [InlineData(MigrationType.COPY_SINGLE)]
        [InlineData(MigrationType.MOVE_SINGLE)]
        [InlineData(MigrationType.COPY_MANY)]
        [InlineData(MigrationType.MOVE_MANY)]
        public void MigrationType_Enum_HasExpectedValues(MigrationType type)
        {
            Assert.Contains(type, new[]
            {
                MigrationType.COPY_SINGLE,
                MigrationType.MOVE_SINGLE,
                MigrationType.COPY_MANY,
                MigrationType.MOVE_MANY
            });
        }

        [Theory]
        [InlineData(MigrationType.COPY_SINGLE, "COPY_SINGLE")]
        [InlineData(MigrationType.MOVE_SINGLE, "MOVE_SINGLE")]
        [InlineData(MigrationType.COPY_MANY, "COPY_MANY")]
        [InlineData(MigrationType.MOVE_MANY, "MOVE_MANY")]
        public void MigrationType_ToString_And_Parse_Works(MigrationType type, string str)
        {
            // ToString
            Assert.Equal(str, MigrationTypeExtensions.ToStringFast(type));
            // Parse
            Assert.Equal(type, MigrationTypeExtensions.Parse(str));
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData(null)]
        public void MigrationType_Parse_Invalid_Throws(string? str)
        {
            Assert.Throws<ArgumentException>(() => MigrationTypeExtensions.Parse(str));
        }
    }
}
