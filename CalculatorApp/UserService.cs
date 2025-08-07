using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<bool> SaveUserAsync(User user);
    }

    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetUserGreetingAsync(int id)
        {
            var user = await _repository.GetUserByIdAsync(id);
            if (user == null)
                return "User not found";
            return $"Hello, {user.Name}!";
        }

        public async Task<bool> CreateUserAsync(string name)
        {
            var user = new User { Name = name };
            return await _repository.SaveUserAsync(user);
        }
    }
}
