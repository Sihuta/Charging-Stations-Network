using ChargingStationsApp.ViewModels.Admin.Admins;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin.Admins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminsPage : ContentPage
    {
        private readonly AdminsViewModel viewModel;

        public AdminsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AdminsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}