using ChargingStationsApp.Models;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface IChargingService
    {
        Task<bool> VehicleIsPluggedInAsync(Station station);
        Task<bool> RequestPaymentAsync(Transaction transaction);
        Task<bool> StartChargingAsync(Station station, double requestedEnergy);
        Task<double> GetProgressAsync(Station station);
        Task<double> StopChargingAsync(Station station);
    }
}
