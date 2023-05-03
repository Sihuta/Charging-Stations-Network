using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Admin.Stations
{
    internal class AddStationViewModel : BaseViewModel
    {
        private string name;
        private double latitude;
        private double longitude;
        private ConnectorType connectorType;

        private readonly Station station;
        private readonly IStationService stationService;

        public AddStationViewModel()
        {
            station = new Station();
            stationService = DependencyService.Get<IStationService>();

            ConnectorTypes = new ObservableCollection<ConnectorType>();

            CancelCommand = new Command(
                async (_) => await OnCancelAsync());
            SaveCommand = new Command(
                async (_) => await OnSaveAsync(), ValidateSave);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public ObservableCollection<ConnectorType> ConnectorTypes { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                station.Name = value;
            }
        }

        public string Latitude
        {
            get => latitude.ToString();
            set
            {
                if (double.TryParse(value, out double val))
                {
                    SetProperty(ref latitude, val);
                    station.Latitude = val;
                }
            }
        }

        public string Longtitude
        {
            get => longitude.ToString();
            set
            {
                if (double.TryParse(value, out double val))
                {
                    SetProperty(ref longitude, val);
                    station.Longitude = val;
                }
            }
        }

        public ConnectorType ConnectorType
        {
            get => connectorType;
            set
            {
                SetProperty(ref connectorType, value);
                station.ConnectorType = connectorType;
            }
        }

        public async Task OnAppearing()
        {
            await LoadConnectorTypesAsync();
        }

        private async Task LoadConnectorTypesAsync()
        {
            var connectorTypes = await stationService.GetConnectorTypesAsync();
            foreach (var ct in connectorTypes)
            {
                ConnectorTypes.Add(ct);
            }
        }

        private bool ValidateSave(object _)
        {
            return !string.IsNullOrWhiteSpace(name)
                && latitude != 0
                && longitude != 0
                && connectorType != null;
        }

        private async Task OnSaveAsync()
        {
            var staId = await stationService.CreateStationAsync(station);
            if (staId == 0)
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveFailTitle", "FailMsg");
            }
            else
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveSuccessTitle", "StationAddedMsg", staId);
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
