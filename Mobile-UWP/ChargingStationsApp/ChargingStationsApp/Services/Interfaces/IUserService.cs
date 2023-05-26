using ChargingStationsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface IUserService
    {
        string AdminRole { get; }
        string ClientRole { get; }

        Task<User> LoginAsync(string email, string password);
        Task<User> RegisterAsync(string email, string password);
        Task<ICollection<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
