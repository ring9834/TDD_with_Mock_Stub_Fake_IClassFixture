using CalculatorApp;
using Moq;
using Xunit;

namespace CalculatorApp.Tests
{
    public class CalculatorTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            _mockLogger = new Mock<ILogger>();
            _calculator = new Calculator(_mockLogger.Object);
        }

        [Fact]
        public void Add_TwoNumbers_ReturnsSum()
        {
            // Arrange
            //var calculator = new Calculator();
            int a = 2, b = 3;
            int expected = 5;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
            _mockLogger.Verify(logger => logger.Log($"Adding {a} and {b}"), Times.Once());
        }

        [Fact]
        public void Add_NegativeNumbers_ReturnsCorrectSum()
        {
            //var calculator = new Calculator();
            int a = -2, b = -3;
            int expected = -5;
            int result = _calculator.Add(a, b);
            Assert.Equal(expected, result);
        }

        //[Fact]
        //public void Add_ZeroAndNumber_ReturnsNumber()
        //{
        //    var calculator = new Calculator();
        //    int a = 0, b = 7;
        //    int expected = 7;
        //    int result = calculator.Add(a, b);
        //    Assert.Equal(expected, result);
        //}

        //[Fact]
        //public void Divide_ValidNumbers_ReturnsQuotient()
        //{
        //    var calculator = new Calculator();
        //    int a = 10, b = 2;
        //    int expected = 5;
        //    int result = calculator.Divide(a, b);
        //    Assert.Equal(expected, result);
        //}

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            //var calculator = new Calculator();
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
            _mockLogger.Verify(logger => logger.Log("Attempted division by zero"), Times.Once());
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(-2, -3, -5)]
        [InlineData(0, 7, 7)]
        public void Add_VariousInputs_ReturnsCorrectSum(int a, int b, int expected)
        {
            //var calculator = new Calculator(new Mock<ILogger>().Object);
            int result = _calculator.Add(a, b);
            Assert.Equal(expected, result);
        }
    }
}