using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using ChargingStationsApp.Views.Client.Charging;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Client.Charging
{
    [QueryProperty(nameof(StationId), nameof(StationId))]
    [QueryProperty(nameof(RedirectedUrl), nameof(RedirectedUrl))]
    internal class PaymentViewModel : BaseViewModel
    {
        private int stationId;
        private string redirectedUrl;

        private readonly IChargingService chargingService;

        public PaymentViewModel()
        {
            chargingService = DependencyService.Get<IChargingService>();
        }

        public int StationId
        {
            get => stationId;
            set => stationId = value;
        }

        public string RedirectedUrl
        {
            get => redirectedUrl;
            set => redirectedUrl = value;
        }

        public async Task OnAppearingAsync()
        {
            await Task.Delay(20000);

            var station = SessionInfo.LastTransaction.Session.Station;
            var requestedEnergy = SessionInfo.LastTransaction.RequestedEnergy;

            if (await chargingService
                .StartChargingAsync(station, requestedEnergy))
            {
                await Shell.Current.GoToAsync(
                    $"{nameof(ChargingProgressPage)}?{nameof(ChargingProgressViewModel.StationId)}={station.Id}");
            }
        }
    }
}
