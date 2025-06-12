using System;
using Xunit;

namespace MigrationStrategy.Tests.Unit.Exceptions
{
    public class MigrationExceptionTests
    {
        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            var ex = new MigrationException("test message");
            Assert.Equal("test message", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            var inner = new Exception("inner");
            var ex = new MigrationException("outer", inner);
            Assert.Equal("outer", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }
    }

    // Dummy class for RED phase
    public class MigrationException : Exception
    {
        public MigrationException(string message) : base(message) { }
        public MigrationException(string message, Exception inner) : base(message, inner) { }
    }
}
