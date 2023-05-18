using ChargingStationsApp.ViewModels.Client.Charging;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Client.Charging
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StationsMapPage : ContentPage
	{
        private readonly StationsMapViewModel viewModel;

		public StationsMapPage ()
		{
			InitializeComponent();
            BindingContext = viewModel = new StationsMapViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearingAsync();
        }

        //private void map_MapClicked(object sender, MapClickedEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
        //}

        private async void Pin_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.OnStationTappedAsync(sender as Pin);
        }
    }
}