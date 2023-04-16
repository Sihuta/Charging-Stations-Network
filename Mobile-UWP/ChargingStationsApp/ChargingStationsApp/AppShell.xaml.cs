using ChargingStationsApp.Views.Admin.Stations;
using ChargingStationsApp.Views.Shared;
using ChargingStationsApp.Views.Shared.Profile;
using System;
using Xamarin.Forms;

namespace ChargingStationsApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));

            Routing.RegisterRoute(nameof(AddStationPage), typeof(AddStationPage));
            Routing.RegisterRoute(nameof(StationDetailsPage), typeof(StationDetailsPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
