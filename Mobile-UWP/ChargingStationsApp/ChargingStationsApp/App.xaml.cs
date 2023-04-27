using ChargingStationsApp.Localization;
using ChargingStationsApp.Services.Realizations;
using ChargingStationsApp.Views.Shared;
using Xamarin.Forms;

namespace ChargingStationsApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<Localize>();
            DependencyService.Register<UserService>();
            DependencyService.Register<StationService>();
            DependencyService.Register<TransactionService>();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
