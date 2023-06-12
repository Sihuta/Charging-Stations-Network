using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class ChargingService : IChargingService
    {
        private static double progress = 0;

        public async Task<double> GetProgressAsync(Station station)
        {
            return await Task.FromResult(++progress);
        }

        public async Task<string> RequestPaymentAsync(Transaction transaction)
        {
            return await Task.FromResult("");
        }

        public async Task<bool> StartChargingAsync(Station station, double requestedEnergy)
        {
            progress = 0;
            return await Task.FromResult(true);
        }

        public async Task<double> StopChargingAsync(Station station)
        {
            return await Task.FromResult(progress);
        }

        public async Task<bool> VehicleIsPluggedInAsync(Station station)
        {
            return await Task.FromResult(Convert.ToBoolean(1));
        }
    }
}
