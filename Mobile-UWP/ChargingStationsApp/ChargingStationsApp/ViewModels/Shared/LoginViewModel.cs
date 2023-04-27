using ChargingStationsApp.Extensions;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Shared
{
    internal class LoginViewModel : BaseViewModel
    {
        private string email;
        private string password;

        private readonly IUserService userService;

        public LoginViewModel()
        {
            userService = DependencyService.Get<IUserService>();

            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        public Command LoginCommand { get; }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private bool ValidateLogin(object _)
        {
            return !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(password)
                && email.IsValidEmailAddress();
        }

        private async void OnLoginClicked(object _)
        {
            SessionInfo.User = await userService.LoginAsync(email, password);
            if (SessionInfo.User != null)
            {
                Application.Current.MainPage = new AppShell();
                return;
            }

            await Application.Current.MainPage.DisplayLocalizedAlert(
                "LoginFailTitle", "LoginFailMsg");
        }
    }
}
