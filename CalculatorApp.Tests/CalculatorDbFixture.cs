using CalculatorApp;
using Microsoft.Data.Sqlite;
using Moq;
using System.Data.Common;

namespace CalculatorApp.Tests
{
    public class CalculatorDbFixture : IDisposable
    {
        public Mock<ILogger> MockLogger { get; }
        public IRepository Repository { get; }
        public Calculator_use_Fixture Calculator { get; }
        private readonly SqliteConnection _connection;

        public CalculatorDbFixture()
        {
            // Setup in-memory SQLite database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Create a table for results
            using var command = _connection.CreateCommand();
            command.CommandText = "CREATE TABLE Results (Value INTEGER)";
            command.ExecuteNonQuery();

            // Setup repository
            Repository = new SqliteRepository(_connection);
            MockLogger = new Mock<ILogger>();
            Calculator = new Calculator_use_Fixture(MockLogger.Object, Repository);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }

    public class SqliteRepository : IRepository
    {
        private readonly DbConnection _connection;

        public SqliteRepository(DbConnection connection)
        {
            _connection = connection;
        }

        public void SaveResult(int result)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = "INSERT INTO Results (Value) VALUES (@value)";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@value";
            parameter.Value = result;
            command.Parameters.Add(parameter);
            command.ExecuteNonQuery();
        }
    }
}