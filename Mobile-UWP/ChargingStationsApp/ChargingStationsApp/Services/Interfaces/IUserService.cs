using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface IUserService
    {
        Task<bool> LoginAsync(string email, string password);
    }
}
