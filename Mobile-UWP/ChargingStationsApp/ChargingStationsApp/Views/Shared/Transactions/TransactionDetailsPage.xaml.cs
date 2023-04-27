using ChargingStationsApp.ViewModels.Shared.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Shared.Transactions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionDetailsPage : ContentPage
	{
		private readonly TransactionDetailsViewModel viewModel;

		public TransactionDetailsPage ()
		{
			InitializeComponent();
			BindingContext = viewModel = new TransactionDetailsViewModel();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
			await viewModel.OnAppearing();
        }
    }
}