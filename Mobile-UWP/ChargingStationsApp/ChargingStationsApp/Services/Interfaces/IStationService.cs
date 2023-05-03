using ChargingStationsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    interface IStationService
    {
        Task<ICollection<Station>> GetStationsAsync();
        Task<Station> GetStationAsync(int id);
        Task<int> CreateStationAsync(Station station);
        Task<bool> UpdateStationAsync(Station station);
        Task<bool> DeleteStationAsync(int id);

        Task<ICollection<ConnectorType>> GetConnectorTypesAsync();
    }
}
