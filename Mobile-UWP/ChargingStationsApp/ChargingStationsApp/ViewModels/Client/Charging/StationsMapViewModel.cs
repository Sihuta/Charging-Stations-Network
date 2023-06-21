using ChargingStationsApp.Enums;
using ChargingStationsApp.Extensions;
using ChargingStationsApp.Localization;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.Views.Client.Charging;
using ChargingStationsApp.Views.Shared.Auth;
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

            Pins = new ObservableCollection<Pin>();
            Stations = new ObservableCollection<Station>();

            LoginCommand = new Command(OnLoginClicked);
        }

        public ObservableCollection<Pin> Pins { get; }
        public ObservableCollection<Station> Stations { get; }

        public Command LoginCommand { get; }


        public async Task OnAppearingAsync()
        {
            var stations = await stationService.GetStationsAsync();
            foreach (var sta in stations)
            {
                Stations.Add(sta);
                Pins.Add(new Pin
                {
                    Label = sta.Name,
                    Address = sta.ConnectorType.Name,
                    Position = new Position(sta.Latitude, sta.Longitude)
                });
            }
        }

        public async Task OnStationTappedAsync(Pin pin)
        {
            var station = GetStationByPin(pin);

            if (await DisplayStationDetailsForConnection(station))
            {
                if (SessionInfo.IsGuest)
                {
                    await Application.Current.MainPage.DisplayLocalizedAlert(
                        "GuestConnectionTitle", "StaChargingMsg");
                }
                else if (await TryToConnectAsync(station))
                {
                    await Shell.Current.GoToAsync(
                        $"{nameof(StartChargingPage)}?{nameof(StartChargingViewModel.StationId)}={station.Id}");
                }
            }
        }

        private async Task<bool> TryToConnectAsync(Station station)
        {
            const int prompts = 10;
            for (int i = 0; i < prompts; ++i)
            {
                var connected = await chargingService.VehicleIsPluggedInAsync(station);
                if (connected)
                {
                    return true;
                }
            }

            await Application.Current.MainPage
                .DisplayLocalizedAlert("FailTitle", "ConnectionFailMsg");
            return false;
        }

        private async Task<bool> DisplayStationDetailsForConnection(Station sta)
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

        private void OnLoginClicked(object _)
        {
            Application.Current.MainPage = new LoginPage();
        }

        private Station GetStationByPin(Pin pin)
        {
            var station = Stations.FirstOrDefault(sta =>
                sta.Latitude == pin.Position.Latitude &&
                sta.Longitude == pin.Position.Longitude);

            station.State = SessionInfo.IsGuest
                    ? StationState.Charging
                    : StationState.Ready;

            return station;
        }
    }
}
