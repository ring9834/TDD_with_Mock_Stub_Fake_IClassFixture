using Moq;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;

namespace CalculatorApp.Tests
{
    // When you implement IClassFixture<T>, xUnit
    // Creates a single instance of the class T for the entire lifetime of your test class.
    // Injects it into the constructor of your test class.
    // Allows you to reuse expensive setup logic (such as database connections, API clients, file systems).
    public class CalculatorTests_use_ClassFixture : IClassFixture<CalculatorDbFixture>
    {
        private readonly CalculatorDbFixture _fixture;

        public CalculatorTests_use_ClassFixture(CalculatorDbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Add_TwoNumbers_ReturnsSum_SavesResult()
        {
            // Arrange
            int a = 2, b = 3;
            int expected = 5;

            // Act
            int result = _fixture.Calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
            _fixture.MockLogger.Verify(logger => logger.Log($"Adding {a} and {b}"), Times.Once());

            // Verify database
            using var connection = _fixture.Calculator.GetType()
                .GetField("_connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_fixture.Calculator) as SqliteConnection;
            if (connection != null)
            {
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT Value FROM Results";
                using var reader = command.ExecuteReader();
                Assert.True(reader.Read());
                Assert.Equal(expected, reader.GetInt32(0));
            }
        }
    }
}