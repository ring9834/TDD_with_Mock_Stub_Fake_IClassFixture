namespace CalculatorApp.Tests;

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