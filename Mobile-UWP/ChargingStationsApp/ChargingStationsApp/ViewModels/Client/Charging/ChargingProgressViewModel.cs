using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.ViewModels.Shared.Transactions;
using ChargingStationsApp.Views.Shared.Transactions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Client.Charging
{
    [QueryProperty(nameof(StationId), nameof(StationId))]
    internal class ChargingProgressViewModel : BaseViewModel
    {
        private int stationId;
        private double progress;
        private double chargedEnergy;
        private double requestedEnergy;

        private Station station;

        private readonly IStationService stationService;
        private readonly IChargingService chargingService;
        private readonly CancellationTokenSource cancellationTokenSource;

        public ChargingProgressViewModel()
        {
            stationService = DependencyService.Get<IStationService>();
            chargingService = DependencyService.Get<IChargingService>();
            cancellationTokenSource = new CancellationTokenSource();

            StopChargingCommand = new Command(
                async (_) => await OnStopChargingAsync());
        }

        public Command StopChargingCommand { get; }

        public int StationId
        {
            get => stationId;
            set => stationId = value;
        }

        public double Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        public double ChargedEnergy
        {
            get => chargedEnergy;
            set => SetProperty(ref chargedEnergy, value);
        }

        public double RequestedEnergy
        {
            get => requestedEnergy;
            set => SetProperty(ref requestedEnergy, value);
        }

        public async Task OnAppearingAsync()
        {
            station = await stationService.GetStationAsync(stationId);
            RequestedEnergy = SessionInfo.LastTransaction.RequestedEnergy;

            await UpdateProgressAsync(cancellationTokenSource.Token);
        }

        private async Task UpdateProgressAsync(CancellationToken cancellationToken)
        {
            while (ChargedEnergy < RequestedEnergy)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                ChargedEnergy = await chargingService.GetProgressAsync(station);
                Progress = ChargedEnergy / RequestedEnergy;

                await Task.Delay(2000);
            }

            await GoToTransactionDetailsAsync(ChargedEnergy);
        }

        private async Task OnStopChargingAsync()
        {
            if (await Application.Current.MainPage
                .DisplayLocalizedAlertWithChoice("StopChargingTitle", "StopChargingMsg"))
            {
                cancellationTokenSource.Cancel();

                var chargedEnergy = await chargingService.StopChargingAsync(station);
                await GoToTransactionDetailsAsync(chargedEnergy);
            }
        }

        private async Task GoToTransactionDetailsAsync(double chargedEnergy)
        {
            var trans = SessionInfo.LastTransaction;

            trans.EndDateTime = System.DateTime.Now;
            trans.ChargedEnergy = chargedEnergy;

            await Shell.Current.GoToAsync("../..");
            await Shell.Current.GoToAsync($"///{nameof(TransactionsPage)}/" +
                $"{nameof(TransactionDetailsPage)}?{nameof(TransactionDetailsViewModel.TransactionId)}={trans.Id}");
        }
    }
}
