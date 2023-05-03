using ChargingStationsApp.ViewModels.Admin.Stations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin.Stations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationsPage : ContentPage
    {
        private readonly StationsViewModel viewModel;

        public StationsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StationsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}