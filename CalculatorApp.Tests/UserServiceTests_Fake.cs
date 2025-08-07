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