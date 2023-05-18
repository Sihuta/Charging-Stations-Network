using ChargingStationsApp.Extensions;
using ChargingStationsApp.Localization;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.Views.Client.Charging;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ChargingStationsApp.ViewModels.Client.Charging
{
    class StationsMapViewModel : BaseViewModel
    {
        private readonly IStationService stationService;
        private readonly IChargingService chargingService;

        public StationsMapViewModel()
        {
            stationService = DependencyService.Get<IStationService>();
            chargingService = DependencyService.Get<IChargingService>();

            Stations = new ObservableCollection<MappedStation>();
        }

        public ObservableCollection<MappedStation> Stations { get; }

        public async Task OnAppearingAsync()
        {
            var stations = await stationService.GetStationsAsync();
            foreach (var sta in stations)
            {
                Stations.Add(new MappedStation
                {
                    Name = sta.Name,
                    Position = new Position(sta.Latitude, sta.Longitude),
                    ConnectorType = sta.ConnectorType,
                    State = sta.State
                });
            }
        }

        public async Task OnStationTappedAsync(Pin pin)
        {
            var station = Stations
                .FirstOrDefault(sta => sta.Position == pin.Position);
            if (await DisplayStationDetailsForConnection(station))
            {
                if (await TryToConnectAsync())
                {
                    await Shell.Current.GoToAsync(nameof(StartChargingPage));
                }
            }
        }

        private async Task<bool> TryToConnectAsync()
        {
            const int prompts = 10;
            for (int i = 0; i < prompts; ++i)
            {
                var connected = await chargingService.VehicleIsPluggedInAsync();
                if (connected)
                {
                    return true;
                }
            }

            await Application.Current.MainPage
                .DisplayLocalizedAlert("FailTitle", "ConnectionFailMsg");
            return false;
        }

        private async Task<bool> DisplayStationDetailsForConnection(MappedStation sta)
        {
            string title = TranslateExtension.GetValue("StationDetailsTitle");
            string state = TranslateExtension.GetValue($"State");
            string stateVal = TranslateExtension.GetValue($"{sta.State}State");
            string name = TranslateExtension.GetValue("Name");
            string connectorType = TranslateExtension.GetValue("ConnectorType");
            string maxPower = TranslateExtension.GetValue("MaxPower");
            string powerUnit = TranslateExtension.GetValue("PowerUnit");
            string yesBtn = TranslateExtension.GetValue("Connect");
            string noBtn = TranslateExtension.GetValue("CancelBtn");

            string msg = $"{state}: {stateVal}\n" +
                $"{name}: {sta.Name}\n" +
                $"{connectorType}: {sta.ConnectorType.Name}\n" +
                $"{maxPower}: {sta.ConnectorType.MaxPower} {powerUnit}";

            return await Application.Current.MainPage
                .DisplayAlert(title, msg, yesBtn, noBtn);
        }
    }
}
