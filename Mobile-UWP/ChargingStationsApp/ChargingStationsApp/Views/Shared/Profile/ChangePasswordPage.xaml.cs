using ChargingStationsApp.ViewModels.Shared.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Shared.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePasswordPage : ContentPage
	{
		public ChangePasswordPage ()
		{
			InitializeComponent();
			BindingContext = new ChangePasswordViewModel();
		}
	}
}