# TDD example using Moq,Stub,Fake,and IClassFixture
TDD is a software development approach where we write tests before writing the actual code. The core value of TDD lies in its ability to produce robust, maintainable, and bug-free code by ensuring that every piece of functionality is tested and validated incrementally. It promotes better design, reduces technical debt, and provides confidence in refactoring.

## When to use TDD
TDD shines in projects requiring high reliability, like financial software or APIs, where bugs are costly. It ensures robust, maintainable code through iterative testing. For mission-critical systems such as medical or aerospace software, TDD’s defect reduction (60-90% in some studies) justifies the upfront time investment.

In teams, TDD provides clear specifications via tests, reducing miscommunication and making onboarding easier.

When requirements are clear, TDD helps enforce them systematically. It’s less effective with vague or rapidly changing specs, where tests may need constant rewriting.

## Mocking and Dependency Injection
For more complex systems, we’ll often need to test classes with dependencies. Use a mocking framework like Moq to isolate dependencies.

Here, we add a logging dependency to the Calculator and test it using TDD.

Add the Moq package
```sh
 dotnet add package Moq
```

Create an ILogger interface
```sh
namespace CalculatorApp
{
    public interface ILogger
    {
        void Log(string message);
    }
}
```

Make Calculator.cs accept an ILogger
```sh
public class Calculator
{
    private readonly ILogger _logger;

    public Calculator(ILogger logger)
    {
        _logger = logger;
    }

    public int Add(int a, int b)
    {
        _logger.Log($"Adding {a} and {b}");
        return a + b;
    }

    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            _logger.Log("Attempted division by zero");
            throw new DivideByZeroException();
        }
        _logger.Log($"Dividing {a} by {b}");
        return a / b;
    }
}
```

Update CalculatorTests.cs to use Moq
```sh
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
        public void Add_TwoNumbers_ReturnsSum_LogsOperation()
        {
            // Arrange
            int a = 2, b = 3;
            int expected = 5;

            // Act
            int result = _calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
            _mockLogger.Verify(logger => logger.Log($"Adding {a} and {b}"), Times.Once());
        }

        [Fact]
        public void Divide_ByZero_ThrowsException_LogsError()
        {
            // Arrange
            int a = 10, b = 0;

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(a, b));
            _mockLogger.Verify(logger => logger.Log("Attempted division by zero"), Times.Once());
        }
    }
}
```
## Using Stub
namespace CalculatorApp.Tests;
```sh
// Stubs are simple implementations of a dependency that return predefined data without verifying interactions. 
// They are useful for providing canned responses to isolate the system under test.
// They are lightweight but less flexible than mocks for complex scenarios.
public class StubUserRepository : IUserRepository
{
    public Task<User> GetUserByIdAsync(int id)
    {
        if (id == 1)
            return Task.FromResult(new User { Id = 1, Name = "Alice" });
        return Task.FromResult<User>(null!);
    }

    public Task<bool> SaveUserAsync(User user)
    {
        return Task.FromResult(true);
    }
}

public class UserServiceTests_Stub
{
    [Fact]
    public async Task GetUserGreetingAsync_ReturnsGreetingForValidUser()
    {
        // Arrange
        var stubRepository = new StubUserRepository();
        var userService = new UserService(stubRepository);

        // Act
        var result = await userService.GetUserGreetingAsync(1);

        // Assert
        Assert.Equal("Hello, Alice!", result);
    }
}
```
## Using Fake
```sh
namespace CalculatorApp.Tests;

// Fakes are fully functional but simplified implementations of a dependency, often mimicking real behavior with in-memory data. 
// They are useful for simulating complex logic without hitting an external system.
// Fakes can be used to provide a more realistic environment for testing, especially when you want to test the integration of your service with a repository.
public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>
    {
        new User { Id = 1, Name = "Alice" },
        new User { Id = 2, Name = "Bob" }
    };

    public Task<User> GetUserByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            user = new User(); // or throw an exception if appropriate
        return Task.FromResult(user);
    }

    public Task<bool> SaveUserAsync(User user)
    {
        user.Id = _users.Count + 1;
        _users.Add(user);
        return Task.FromResult(true);
    }
}

public class UserServiceTests_Fake
{
    [Fact]
    public async Task GetUserGreetingAsync_ReturnsGreetingForValidUser()
    {
        // Arrange
        var fakeRepository = new FakeUserRepository();
        var userService = new UserService(fakeRepository);

        // Act
        var result = await userService.GetUserGreetingAsync(1);

        // Assert
        Assert.Equal("Hello, Alice!", result);
    }

    [Fact]
    public async Task CreateUserAsync_AddsUserToFakeRepository()
    {
        // Arrange
        var fakeRepository = new FakeUserRepository();
        var userService = new UserService(fakeRepository);

        // Act
        var result = await userService.CreateUserAsync("Charlie");

        // Assert
        Assert.True(result);
        var newUser = await fakeRepository.GetUserByIdAsync(3);
        Assert.Equal("Charlie", newUser.Name);
    }
}
```

## Parameterized Tests
Use xUnit’s [Theory] and [InlineData] to test multiple inputs.
```sh
[Theory]
[InlineData(2, 3, 5)]
[InlineData(-2, -3, -5)]
[InlineData(0, 7, 7)]
public void Add_VariousInputs_ReturnsCorrectSum(int a, int b, int expected)
{
    var calculator = new Calculator(new Mock<ILogger>().Object);
    int result = calculator.Add(a, b);
    Assert.Equal(expected, result);
}
```

## Best Practices for TDD in C#
Write Clear Tests: Use the Arrange-Act-Assert (AAA) pattern for readability.

One Assertion per Test: Avoid multiple assertions unless testing a single behavior.

Keep Tests Fast: Unit tests should run quickly to maintain productivity.

Avoid Testing Implementation: Test what the code does, not how it does it.

Use Descriptive Test Names: for example, Add_TwoNumbers_ReturnsSum is clear and specific.

Maintain Test Isolation: Tests should not depend on each other or external state.
Refactor Tests: Keep tests clean, just like production code.



