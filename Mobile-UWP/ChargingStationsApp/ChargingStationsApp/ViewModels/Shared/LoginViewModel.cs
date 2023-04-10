using ChargingStationsApp.Localization;
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

            LoginCommand = new Command(OnLogin, ValidateLogin);
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

        private bool ValidateLogin(object obj)
        {
            return !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(password);
        }

        private async void OnLogin(object obj)
        {
            var login = await userService.LoginAsync(email, password);
            if (!login)
            {
                string title = TranslateExtension.GetValue("LoginFailTitle");
                string msg = TranslateExtension.GetValue("LoginFailMsg");

                await Application.Current.MainPage.DisplayAlert(title, msg, "OK");
                return;
            }

            //Debug.WriteLine(res.Role);
            //var employee = await EmployeeService.GetByUserId(res.Id);

            //App.User = res;
            //App.UserPassword = Password;
            //App.EmployeeId = employee.Id;

            Application.Current.MainPage = new AppShell();
        }
    }
}
