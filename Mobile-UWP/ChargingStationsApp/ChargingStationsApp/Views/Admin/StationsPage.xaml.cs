using ChargingStationsApp.ViewModels.Admin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationsPage : ContentPage
    {
        public StationsPage()
        {
            InitializeComponent();
            this.BindingContext = new StationsViewModel();
        }
    }
}