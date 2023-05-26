using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System;

namespace ChargingStationsApp.Services.Realizations
{
    internal class IotChargingService : IChargingService
    {
        private readonly HttpClient httpClient = App.HttpClient;
        private readonly JsonSerializerOptions jsonOptions = App.JsonOptions;

        private double progress = 0;

        public async Task<double> GetProgressAsync(Station station)
        {
            var uri = $"{station.ServerUrl}/progress";

            try
            {
                var result = await httpClient.GetStringAsync(uri);
                progress = JsonSerializer.Deserialize<double>(result, jsonOptions);

            }
            catch
            {
                return progress + 1;
            }

            return progress;
        }

        public async Task<bool> RequestPaymentAsync(Transaction transaction)
        {
            // TODO
            return await Task.FromResult(true);
        }

        public async Task<bool> StartChargingAsync(Station station, double requestedEnergy)
        {
            var uri = $"{station.ServerUrl}/start_charging/{requestedEnergy}";
            var response = await httpClient.PostAsync(uri, null);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<double> StopChargingAsync(Station station)
        {
            var uri = $"{station.ServerUrl}/stop_charging";
            var result = await httpClient.GetStringAsync(uri);

            return JsonSerializer.Deserialize<double>(result, jsonOptions);
        }

        public async Task<bool> VehicleIsPluggedInAsync(Station station)
        {
            var uri = $"{station.ServerUrl}/plugged_in";
            var result = await httpClient.GetStringAsync(uri);

            var boolValue = JsonSerializer.Deserialize<byte>(result, jsonOptions);
            return Convert.ToBoolean(boolValue);
        }
    }
}
