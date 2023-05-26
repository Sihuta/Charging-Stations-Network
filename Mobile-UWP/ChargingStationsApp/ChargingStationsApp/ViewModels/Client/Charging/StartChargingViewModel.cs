using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.Views.Client.Charging;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Client.Charging
{
    [QueryProperty(nameof(StationId), nameof(StationId))]
    internal class StartChargingViewModel : BaseViewModel
    {
        private int stationId;
        private double requestedEnergy;
        private double? approximateTime = 0;
        private decimal? price = 0;

        private Tariff tariff;
        private Station station;

        private readonly IStationService stationService;
        private readonly IChargingService chargingService;

        public StartChargingViewModel()
        {
            stationService = DependencyService.Get<IStationService>();
            chargingService = DependencyService.Get<IChargingService>();

            CancelCommand = new Command(async (_) => await OnCancelCommandAsync());
            PayCommand = new Command(
                async (_) => await OnPayCommandAsync(), ValidatePay);
            PropertyChanged +=
                (_, __) => PayCommand.ChangeCanExecute();
        }

        public Command PayCommand { get; }
        public Command CancelCommand { get; }

        public int StationId
        {
            get => stationId;
            set => stationId = value;
        }

        public double RequestedEnergy
        {
            get => requestedEnergy;
            set
            {
                SetProperty(ref requestedEnergy, value);
                
                ApproximateTime = value / station?.ConnectorType.MaxPower;
                Price = (decimal)value * tariff?.Price;
            }
        }

        public double? ApproximateTime
        {
            get => approximateTime;
            set => SetProperty(ref approximateTime, value);
        }

        public decimal? Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public async Task OnAppearingAsync()
        {
            station = await stationService.GetStationAsync(stationId);
            tariff = await stationService.GetTariffForStationAsync(stationId);
        }

        private async Task OnPayCommandAsync()
        {
            SessionInfo.LastTransaction = new Transaction
            {
                Session = new Session
                {
                    Station = station,
                    UserId = SessionInfo.User.Id,
                },
                RequestedEnergy = requestedEnergy,
                Tariff = tariff,
                StartDateTime = DateTime.Now,
            };

            if (await chargingService.RequestPaymentAsync(SessionInfo.LastTransaction) &&
                await chargingService.StartChargingAsync(station, requestedEnergy))
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(ChargingProgressPage)}?{nameof(ChargingProgressViewModel.StationId)}={station.Id}");
            }
        }

        private bool ValidatePay(object _)
        {
            return requestedEnergy >= 1;
        }

        private async Task OnCancelCommandAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
