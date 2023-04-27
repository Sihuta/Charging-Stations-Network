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

        private Station oldStation;
        private readonly Station newStation;
        private readonly IStationService stationService;

        public StationDetailsViewModel()
        {
            newStation = new Station();
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
                newStation.Id = value;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
                newStation.Name = value;
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
                    newStation.Latitude = val;
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
                    newStation.Longitude = val;
                }
            }
        }

        public ConnectorType ConnectorType
        {
            get => connectorType;
            set
            {
                SetProperty(ref connectorType, value);
                newStation.ConnectorType = connectorType;
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
            oldStation = await stationService.GetStationAsync(stationId);

            Name = oldStation.Name;
            Latitude = oldStation.Latitude.ToString();
            Longtitude = oldStation.Longitude.ToString();
            ConnectorType = oldStation.ConnectorType;
        }

        private bool ValidateSave(object _)
        {
            return !string.IsNullOrWhiteSpace(name)
                && latitude != 0
                && longitude != 0
                && connectorType != null

                && (name != oldStation.Name
                || latitude != oldStation.Latitude
                || longitude != oldStation.Longitude
                || connectorType != oldStation.ConnectorType);
        }

        private async Task OnSaveAsync()
        {
            var res = await stationService.UpdateStationAsync(newStation);
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
