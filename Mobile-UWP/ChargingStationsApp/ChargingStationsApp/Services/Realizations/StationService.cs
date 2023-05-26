using ChargingStationsApp.Enums;
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
                MaxPower = 7
            },
            new ConnectorType
            {
                Name = "Connector Type 2",
                MaxPower = 21
            }
        };

        public static readonly List<Station> Stations = new List<Station>
        {
            new Station
            {
                Id = 1,
                Name = "Station 1",
                Latitude = 50.0156448,
                Longitude = 36.2270381,
                ServerUrl = "serverUrl",
                WifiSsid = "wifiSsid",
                WifiPwd = "wifiPwd",
                ConnectorType = connectorTypes[0],
                State = StationState.Ready
            },
            //new Station
            //{
            //    Id = 2,
            //    Name = "Station 2",
            //    Latitude = 50.0140603,
            //    Longitude = 36.2274672,
            //    ServerUrl = "serverUrl",
            //    WifiSsid = "wifiSsid",
            //    WifiPwd = "wifiPwd",
            //    ConnectorType = connectorTypes[1],
            //    State = StationState.Error
            //}
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
            return await Task.FromResult(Stations.Find(sta => sta.Id == id));
        }

        public async Task<ICollection<Station>> GetStationsAsync()
        {
            return await Task.FromResult(Stations);
        }

        public async Task<bool> UpdateStationAsync(Station station)
        {
            return await Task.FromResult(true);
        }

        public async Task<Tariff> GetTariffForStationAsync(int stationId)
        {
            return await Task.FromResult(TransactionService.Tariff);
        }
    }
}
