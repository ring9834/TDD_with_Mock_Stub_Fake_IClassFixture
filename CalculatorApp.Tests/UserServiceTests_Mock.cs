using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Tests
{
    // Mocks allow you to define specific behavior (e.g., return null for a missing user) and verify interactions (e.g., ensure a method was called).
    // Best for testing the behavior of your code, like whether a method was called correctly.
    public class UserServiceTests_Mock
    {
        [Fact]
        public async Task GetUserGreetingAsync_UserNotFound_ReturnsNotFoundMessage()
        {
            // Arrange  
            var mockRepository = new Mock<IUserRepository>();
            // This sets up the mock to define what happens when the GetUserByIdAsync method is called with the argument 1.
            mockRepository.Setup(repo => repo.GetUserByIdAsync(1))
                          // Specifies that when GetUserByIdAsync(1) is called, the mock should return null asynchronously.
                          .ReturnsAsync((User?)null!); // Ensure Task<User?> is returned
                                                       // null!: The null! syntax explicitly tells the C# compiler to treat the null value as non-null for the purposes of 
                                                       // nullability checks. This is necessary because the ReturnsAsync method expects a value that matches the return type 
                                                       // of GetUserByIdAsync, which is Task<User?>. The User? type allows null, but the compiler might still issue a warning 
                                                       // or error if it detects a potential null reference issue in strict nullable contexts. By adding !, you’re asserting 
                                                       // to the compiler, "I know this is null, but treat it as a valid non-null value for this context."

            var userService = new UserService(mockRepository.Object);

            // Act  
            var result = await userService.GetUserGreetingAsync(1);

            // Assert  
            Assert.Equal("User not found", result);
            // The Verify method in Moq is used to check whether a specific method on the mocked object was called during the execution of the test.
            // repo => repo.GetUserByIdAsync(1) specifies the method and arguments to verify. Here, it checks for calls to the GetUserByIdAsync method on the mocked repository with the argument 1.
            // Times.Once() specifies that the method should have been called exactly once during the test execution.
            mockRepository.Verify(repo => repo.GetUserByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task CreateUserAsync_SavesUser_CallsRepository()
        {
            // Arrange  
            var mockRepository = new Mock<IUserRepository>();
            mockRepository.Setup(repo => repo.SaveUserAsync(It.IsAny<User>()))
                          .ReturnsAsync(true);
            var userService = new UserService(mockRepository.Object);

            // Act  
            var result = await userService.CreateUserAsync("Alice");

            // Assert  
            Assert.True(result);
            mockRepository.Verify(repo => repo.SaveUserAsync(It.IsAny<User>()), Times.Once());
        }
    }
}
