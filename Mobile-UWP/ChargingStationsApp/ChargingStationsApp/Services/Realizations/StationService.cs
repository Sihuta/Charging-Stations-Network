using ChargingStationsApp.Enums;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Realizations
{
    class StationService : IStationService
    {
        private static readonly List<ConnectorType> connectorTypes = new List<ConnectorType>
        {
            new ConnectorType
            {
                Name = "SAE J1772 Тип 1",
                MaxPower = 7
            },
            new ConnectorType
            {
                Name = "IEC 62196 Тип 2",
                MaxPower = 21
            }
        };

        public static readonly List<Station> Stations = new List<Station>
        {
            new Station
            {
                Id = 1,
                Name = "Station 2",
                Latitude = 50.0156448,
                Longitude = 36.2270381,
                ServerUrl = "https://192.168.1.8/esp32/api",
                WifiSsid = "wifiSsid",
                WifiPwd = "wifiPwd",
                ConnectorType = connectorTypes[1],
                State = StationState.Ready
            },
        };

        public async Task<int> CreateStationAsync(Station station)
        {
            int id = Stations.Last().Id + 1;
            station.Id = id;
            Stations.Add(station);

            return await Task.FromResult(id);
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            var ind = Stations.FindIndex(s => s.Id == id);
            Stations.RemoveAt(ind);

            return await Task.FromResult(true);
        }

        public async Task<ICollection<ConnectorType>> GetConnectorTypesAsync()
        {
            return await Task.FromResult(connectorTypes);
        }

        public async Task<Station> GetStationAsync(int id)
        {
            return await Task.FromResult(
                Stations.Find(sta => sta.Id == id));
        }

        public async Task<ICollection<Station>> GetStationsAsync()
        {
            return await Task.FromResult(Stations);
        }

        public async Task<bool> UpdateStationAsync(Station station)
        {
            var updStation = Stations.Find(s => s.Id == station.Id);

            updStation.Name = station.Name;
            updStation.Latitude = station.Latitude;
            updStation.Longitude = station.Longitude;
            updStation.ConnectorType = station.ConnectorType;

            return await Task.FromResult(true);
        }

        public async Task<Tariff> GetTariffForStationAsync(int stationId)
        {
            return await Task.FromResult(TransactionService.Tariff);
        }
    }
}
