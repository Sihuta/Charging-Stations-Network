using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface IChargingService
    {
        Task<bool> VehicleIsPluggedInAsync();
    }
}
