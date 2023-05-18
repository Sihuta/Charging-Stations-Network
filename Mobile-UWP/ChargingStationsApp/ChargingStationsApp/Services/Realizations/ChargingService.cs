using ChargingStationsApp.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    internal class ChargingService : IChargingService
    {
        public async Task<bool> VehicleIsPluggedInAsync()
        {
            return await Task.FromResult(Convert.ToBoolean(1));
        }
    }
}
