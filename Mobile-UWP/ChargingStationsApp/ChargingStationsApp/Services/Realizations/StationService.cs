using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    class StationService : IStationService
    {
        private static readonly List<ConnectorType> connectorTypes = new List<ConnectorType>
        {
            new ConnectorType
            {
                Name = "Connector Type 1",
                MaxPower = 10
            },
            new ConnectorType
            {
                Name = "Connector Type 2",
                MaxPower = 20
            }
        };

        private static readonly List<Station> stations = new List<Station>
        {
            new Station
            {
                Id = 1,
                Name = "Station 1",
                Latitude = -34.397,
                Longitude = 150.644,
                ServerUrl = "serverUrl",
                WifiSsid = "wifiSsid",
                WifiPwd = "wifiPwd",
                ConnectorType = connectorTypes[0]
            },
            new Station
            {
                Id = 2,
                Name = "Station 2",
                Latitude = -34.397,
                Longitude = 150.644,
                ServerUrl = "serverUrl",
                WifiSsid = "wifiSsid",
                WifiPwd = "wifiPwd",
                ConnectorType = connectorTypes[1]
            }
        };

        public async Task<int> CreateStationAsync(Station station)
        {
            return await Task.FromResult(1);
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            return await Task.FromResult(true);
        }

        public async Task<ICollection<ConnectorType>> GetConnectorTypesAsync()
        {
            return await Task.FromResult(connectorTypes);
        }

        public async Task<Station> GetStationAsync(int id)
        {
            return await Task.FromResult(stations.Find(sta => sta.Id == id));
        }

        public async Task<ICollection<Station>> GetStationsAsync()
        {
            return await Task.FromResult(stations);
        }

        public async Task<bool> UpdateStationAsync(Station station)
        {
            return await Task.FromResult(true);
        }
    }
}
