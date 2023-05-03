using ChargingStationsApp.ViewModels.Admin.Admins;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Admin.Admins
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAdminPage : ContentPage
    {
        public AddAdminPage()
        {
            InitializeComponent();
            BindingContext = new AddAdminViewModel();
        }
    }
}