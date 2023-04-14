using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class UserService : IUserService
    {
        private User testUser = new User
        {
            Id = 0,
            Email = "user@example.com",
            Password = "password",
            Role = "admin"
        };

        public async Task<User> LoadUserAsync(int id)
        {
            return await Task.FromResult(testUser);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await Task.FromResult(testUser);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await Task.FromResult(true);
        }
    }
}
