using ChargingStationsApp.Localization;
using ChargingStationsApp.Services.Realizations;
using ChargingStationsApp.Views.Shared;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;

namespace ChargingStationsApp
{
    public partial class App : Application
    {
        public static HttpClient HttpClient { get; private set; }
        public static JsonSerializerOptions JsonOptions
        {
            get => new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Localize>();
            DependencyService.Register<UserService>();
            DependencyService.Register<StationService>();
            DependencyService.Register<TransactionService>();
            DependencyService.Register<IotChargingService>();

            MainPage = new LoginPage();
            HttpClient = CreateHttpClient();
        }

        public static HttpContent GetHttpContent(object obj)
        {
            return new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8, "application/json");
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
