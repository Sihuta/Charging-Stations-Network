using ChargingStationsApp.Localization;
using ChargingStationsApp.Services.Realizations;
using ChargingStationsApp.Views.Shared;
using Xamarin.Forms;

namespace ChargingStationsApp
{
    public partial class App : Application
    {
        public static bool IsAdmin { get => true; }
        public static bool IsUser { get => false; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Localize>();
            DependencyService.Register<UserService>();

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
