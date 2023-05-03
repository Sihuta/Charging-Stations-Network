using ChargingStationsApp.ViewModels.Shared.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Shared.Transactions
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchOptionsPage : ContentPage
	{
		private readonly SearchOptionsViewModel viewModel;

		public SearchOptionsPage()
		{
			InitializeComponent();
			BindingContext = viewModel = new SearchOptionsViewModel();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
			await viewModel.OnAppearing();

			SetUpDatePicker();
		}

		private void SetUpDatePicker()
		{
			fromDatePicker.Date = viewModel.DateFrom;
            toDatePicker.Date = viewModel.DateTo;

            fromDatePicker.MaximumDate =
				toDatePicker.MaximumDate = viewModel.MaxDate;
		}

		private void OnDateFromSelected(object sender, DateChangedEventArgs args)
		{
			viewModel.DateFrom = args.NewDate;
		}

        private void OnDateToSelected(object sender, DateChangedEventArgs args)
        {
			viewModel.DateTo = args.NewDate;
        }
    }
}