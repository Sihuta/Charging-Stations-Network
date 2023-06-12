using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.Views.Client.Charging;
using ChargingStationsApp.Views.Shared.Profile;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Shared.Profile
{
    [QueryProperty(nameof(UserId), nameof(UserId))]
    internal class ProfileViewModel : BaseViewModel
    {
        private int userId;
        private string email;
        private string password;
        private string role;

        private readonly IUserService userService;

        public ProfileViewModel(int userId)
        {
            UserId = userId;
            User = new User();
            userService = DependencyService.Get<IUserService>();

            LogoutCommand = new Command(OnLogoutClicked);
            ChangePasswordCommand = new Command(OnChangePasswordClicked);
            SaveCommand = new Command(OnSaveClicked, ValidateSave);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command LogoutCommand { get; }
        public Command SaveCommand { get; }
        public Command ChangePasswordCommand { get; }

        public User User { get; }

        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
                User.Email = value;
            }
        }

        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                User.Password = value;
            }
        }

        public string Role
        {
            get => role;
            set
            {
                SetProperty(ref role, value);
                User.Role = value;
            }
        }

        internal async Task OnAppearing()
        {
            await LoadUserAsync(userId);
        }

        private void OnLogoutClicked()
        {
            SessionInfo.Logout();
            Application.Current.MainPage = new StationsMapPage();
        }

        private async Task LoadUserAsync(int id)
        {
            var user = await userService.GetUserAsync(id);

            Email = user.Email;
            Password = user.Password;
            Role = user.Role;

            SessionInfo.User = user;
        }

        private bool ValidateSave(object _)
        {
            return !string.IsNullOrWhiteSpace(email)
                && email != SessionInfo.User.Email
                && email.IsValidEmailAddress();
        }

        private async void OnSaveClicked(object _)
        {
            User.Id = userId;

            var saved = await userService.UpdateUserAsync(User);
            if (saved)
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveSuccessTitle", "SaveSuccessMsg");
            }
            else
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveFailTitle", "SaveProfileFailMsg");
            }
        }

        private async void OnChangePasswordClicked(object _)
        {
            await Shell.Current.GoToAsync(nameof(ChangePasswordPage));
        }
    }
}
