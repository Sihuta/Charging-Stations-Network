using ChargingStationsApp.Services.Interfaces;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class UserService : IUserService
    {
        public async Task<bool> LoginAsync(string email, string password)
        {
            return true;
        }
    }
}
