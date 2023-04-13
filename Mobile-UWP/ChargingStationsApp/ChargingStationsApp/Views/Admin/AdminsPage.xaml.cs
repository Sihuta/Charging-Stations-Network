using ChargingStationsApp.ViewModels.Admin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminsPage : ContentPage
    {
        public AdminsPage()
        {
            InitializeComponent();
            this.BindingContext = new AdminsViewModel();
        }
    }
}