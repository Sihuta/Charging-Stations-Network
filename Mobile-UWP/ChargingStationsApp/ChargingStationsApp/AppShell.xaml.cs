using ChargingStationsApp.Views.Admin.Admins;
using ChargingStationsApp.Views.Admin.Stations;
using ChargingStationsApp.Views.Client.Charging;
using ChargingStationsApp.Views.Shared.Auth;
using ChargingStationsApp.Views.Shared.Profile;
using ChargingStationsApp.Views.Shared.Transactions;
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
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
            Routing.RegisterRoute(nameof(TransactionDetailsPage), typeof(TransactionDetailsPage));
            Routing.RegisterRoute(nameof(SearchOptionsPage), typeof(SearchOptionsPage));

            Routing.RegisterRoute(nameof(AddStationPage), typeof(AddStationPage));
            Routing.RegisterRoute(nameof(StationDetailsPage), typeof(StationDetailsPage));
            Routing.RegisterRoute(nameof(AddAdminPage), typeof(AddAdminPage));

            Routing.RegisterRoute(nameof(StartChargingPage), typeof(StartChargingPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(ChargingProgressPage), typeof(ChargingProgressPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
