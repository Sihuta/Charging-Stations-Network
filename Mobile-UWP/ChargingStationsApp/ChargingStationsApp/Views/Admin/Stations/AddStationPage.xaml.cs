using ChargingStationsApp.ViewModels.Admin.Stations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin.Stations
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStationPage : ContentPage
    {
        private readonly AddStationViewModel viewModel;

        public AddStationPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AddStationViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}