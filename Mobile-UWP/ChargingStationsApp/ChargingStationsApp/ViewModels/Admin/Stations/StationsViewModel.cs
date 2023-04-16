using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.Views.Admin.Stations;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Admin.Stations
{
    internal class StationsViewModel : BaseViewModel
    {
        private Station selectedStation;
        private readonly IStationService stationService;

        public StationsViewModel()
        {
            stationService = DependencyService.Get<IStationService>();

            Stations = new ObservableCollection<Station>();

            LoadStationsCommand = new Command(
                async () => await LoadStationsAsync());
            AddStationCommand = new Command(
                async () => await OnAddStationAsync());

            StationTapped = new Command<Station>(
                async (station) => await OnStationTappedAsync(station));
            RemoveStation = new Command<int>(
                async (id) => await OnRemoveStationAsync(id));

        }

        public ObservableCollection<Station> Stations { get; }
        public Command LoadStationsCommand { get; }
        public Command AddStationCommand { get; }
        public Command<Station> StationTapped { get; }
        public Command<int> RemoveStation { get; }

        public Station SelectedStation
        {
            get => selectedStation;
            set => SetProperty(ref selectedStation, value);
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedStation = null;
        }

        private async Task LoadStationsAsync()
        {
            IsBusy = true;

            try
            {
                Stations.Clear();
                var stations = await stationService.GetStationsAsync();
                foreach (var sta in stations)
                {
                    Stations.Add(sta);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnAddStationAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddStationPage));
        }

        private async Task OnStationTappedAsync(Station station)
        {
            if (station is null)
            {
                return;
            }

            await Shell.Current.GoToAsync(
                $"{nameof(StationDetailsPage)}?{nameof(StationDetailsViewModel.StationId)}={station.Id}");
        }

        private async Task OnRemoveStationAsync(int id)
        {
            if (await Application.Current.MainPage
                .DisplayLocalizedAlertWithChoice("ConfirmRemoveTitle", "ConfirmRemoveMsg"))
            {
                await stationService.DeleteStationAsync(id);
                IsBusy = true;
            }
        }
    }
}
