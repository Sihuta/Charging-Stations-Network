using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Admin.Stations
{
    [QueryProperty(nameof(StationId), nameof(StationId))]
    internal class StationDetailsViewModel : BaseViewModel
    {
        private int stationId;
        private string name;
        private double latitude;
        private double longitude;
        private ConnectorType connectorType;

        private Station station;
        private readonly Station updStation;
        private readonly IStationService stationService;

        public StationDetailsViewModel()
        {
            updStation = new Station();
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

        public int StationId
        {
            get => stationId;
            set
            {
                stationId = value;
                updStation.Id = value;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                updStation.Name = value;
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
                    updStation.Latitude = val;
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
                    updStation.Longitude = val;
                }
            }
        }

        public ConnectorType ConnectorType
        {
            get => connectorType;
            set
            {
                SetProperty(ref connectorType, value);
                updStation.ConnectorType = connectorType;
            }
        }

        public async Task OnAppearing()
        {
            await LoadConnectorTypesAsync();
            await LoadStationAsync();
        }

        private async Task LoadConnectorTypesAsync()
        {
            var connectorTypes = await stationService.GetConnectorTypesAsync();
            foreach (var ct in connectorTypes)
            {
                ConnectorTypes.Add(ct);
            }
        }

        private async Task LoadStationAsync()
        {
            station = await stationService.GetStationAsync(stationId);

            Name = station.Name;
            Latitude = station.Latitude.ToString();
            Longtitude = station.Longitude.ToString();
            ConnectorType = station.ConnectorType;
        }

        private bool ValidateSave(object _)
        {
            return !string.IsNullOrWhiteSpace(name)
                && latitude != 0
                && longitude != 0
                && connectorType != null

                && (name != station.Name
                || latitude != station.Latitude
                || longitude != station.Longitude
                || connectorType != station.ConnectorType);
        }

        private async Task OnSaveAsync()
        {
            var res = await stationService.UpdateStationAsync(updStation);
            if (res)
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveSuccessTitle", "SaveSuccessMsg");
            }
            else
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveFailTitle", "FailMsg");
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
