using ChargingStationsApp.Extensions;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.Services;
using Xamarin.Forms;
using ChargingStationsApp.Views.Shared.Auth;

namespace ChargingStationsApp.ViewModels.Shared.Auth
{
    internal class RegisterViewModel : BaseViewModel
    {
        private string email;
        private string password;
        private string confirmPassword;

        private readonly IUserService userService;

        public RegisterViewModel()
        {
            userService = DependencyService.Get<IUserService>();

            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked, ValidateRegister);
            PropertyChanged +=
                (_, __) => RegisterCommand.ChangeCanExecute();
        }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

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

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private bool ValidateRegister(object _)
        {
            return !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(password)
                && !string.IsNullOrWhiteSpace(confirmPassword)
                && email.IsValidEmailAddress();
        }

        private void OnLoginClicked(object _)
        {
            Application.Current.MainPage = new LoginPage();
        }

        private async void OnRegisterClicked(object _)
        {
            if (!confirmPassword.Equals(password))
            {
                await Application.Current.MainPage
                    .DisplayLocalizedAlert("InvalidInputTitle", "InvalidInputNewPassMsg");
                return;
            }

            SessionInfo.User = await userService.RegisterAsync(email, password);
            if (SessionInfo.User != null)
            {
                Application.Current.MainPage = new AppShell();
                return;
            }

            await Application.Current.MainPage.DisplayLocalizedAlert(
                "SaveFailTitle", "SaveProfileFailMsg");
        }
    }
}
