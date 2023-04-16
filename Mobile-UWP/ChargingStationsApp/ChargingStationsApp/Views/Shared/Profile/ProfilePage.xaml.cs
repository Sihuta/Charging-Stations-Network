using ChargingStationsApp.Services;
using ChargingStationsApp.ViewModels.Shared.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Shared.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel viewModel;

        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProfileViewModel(SessionInfo.User.Id);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}