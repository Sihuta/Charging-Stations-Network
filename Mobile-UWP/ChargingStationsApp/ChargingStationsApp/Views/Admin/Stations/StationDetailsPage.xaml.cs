using ChargingStationsApp.ViewModels.Admin.Stations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin.Stations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationDetailsPage : ContentPage
    {
        private readonly StationDetailsViewModel viewModel;

        public StationDetailsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StationDetailsViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}