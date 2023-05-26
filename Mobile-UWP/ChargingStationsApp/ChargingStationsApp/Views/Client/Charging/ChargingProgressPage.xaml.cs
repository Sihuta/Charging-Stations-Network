using ChargingStationsApp.ViewModels.Client.Charging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Client.Charging
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChargingProgressPage : ContentPage
    {
        private readonly ChargingProgressViewModel viewModel;

        public ChargingProgressPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ChargingProgressViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearingAsync();
        }
    }
}