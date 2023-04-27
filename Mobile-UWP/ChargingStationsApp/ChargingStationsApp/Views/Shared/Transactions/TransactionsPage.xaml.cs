using ChargingStationsApp.ViewModels.Shared.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Shared.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsPage : ContentPage
    {
        private readonly TransactionsViewModel viewModel;

        public TransactionsPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new TransactionsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}