using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private readonly Station station;
        private readonly IStationService stationService;

        public StationDetailsViewModel()
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

        public int StationId
        {
            get => stationId;
            set => stationId = value;
        }

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
            var sta = await stationService.GetStationAsync(stationId);

            Name = sta.Name;
            Latitude = sta.Latitude.ToString();
            Longtitude = sta.Longitude.ToString();
            ConnectorType = sta.ConnectorType;
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
            var res = await stationService.UpdateStationAsync(station);
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
