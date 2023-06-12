using ChargingStationsApp.ViewModels.Client.Charging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Client.Charging
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPage : ContentPage
    {
        private readonly PaymentViewModel viewModel;

        public PaymentPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new PaymentViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = viewModel.RedirectedUrl
                }
            };

            Content = webView;

            await viewModel.OnAppearingAsync();
        }
    }
}