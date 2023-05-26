using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class UserService : IUserService
    {
        private static readonly List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Email = "user@example.com",
                Password = "password",
                Role = "client"
            },
            new User
            {
                Id = 2,
                Email = "admin@example.com",
                Password = "password",
                Role = "admin"
            },
        };

        public string AdminRole => "admin";

        public string ClientRole => "client";

        public async Task<bool> CreateUserAsync(User user)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await Task.FromResult(true);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await Task.FromResult(users[0]);
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await Task.FromResult(users[0]);
        }

        public async Task<User> RegisterAsync(string email, string password)
        {
            return await Task.FromResult(users[0]);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await Task.FromResult(true);
        }
    }
}
