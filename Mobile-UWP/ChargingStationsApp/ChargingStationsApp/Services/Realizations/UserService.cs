using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
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
                Email = "client@example.com",
                Password = "12345678",
                Role = "client"
            },
            new User
            {
                Id = 2,
                Email = "admin@example.com",
                Password = "12345678",
                Role = "admin"
            },
        };

        public string AdminRole => "admin";

        public string ClientRole => "client";

        public async Task<bool> CreateUserAsync(User user)
        {
            user.Id = users.Last().Id + 1;
            users.Add(user);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var ind = users.FindIndex(u => u.Id == id);
            users.RemoveAt(ind);

            return await Task.FromResult(true);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await Task.FromResult(
                users.SingleOrDefault(u => u.Id == id));
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            return await Task.FromResult(users);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await Task.FromResult(
                users.SingleOrDefault(u => u.Email == email && u.Password == password));
        }

        public async Task<User> RegisterAsync(string email, string password)
        {
            var user = new User
            {
                Id = users.Last().Id + 1,
                Email = email,
                Password = password,
                Role = ClientRole,
            };
            users.Add(user);

            return await Task.FromResult(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var updUser = users.Find(u => u.Id == user.Id);
            
            updUser.Email = user.Email;
            updUser.Password = user.Password;

            return await Task.FromResult(true);
        }

        public async Task<bool> UserExists(string email)
        {
            var res = users.Any(u => u.Email == email);
            return await Task.FromResult(res);
        }
    }
}
