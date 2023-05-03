using ChargingStationsApp.Extensions;
using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Admin.Admins
{
    internal class AddAdminViewModel : BaseViewModel
    {
        private string email;
        private string password;

        private readonly User admin;
        private readonly IUserService userService;

        public AddAdminViewModel()
        {
            userService = DependencyService.Get<IUserService>();
            admin = new User
            {
                Role = userService.AdminRole
            };

            CancelCommand = new Command(
                async (_) => await OnCancelAsync());
            SaveCommand = new Command(
                async (_) => await OnSaveAsync(), ValidateSave);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);
                admin.Email = value;
            }
        }

        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                admin.Password = value;
            }
        }

        private bool ValidateSave(object _)
        {
            return !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(password)
                && email.IsValidEmailAddress();
        }

        private async Task OnSaveAsync()
        {
            var res = await userService.CreateUserAsync(admin);
            if (res)
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveSuccessTitle", "SaveSuccessMsg");
            }
            else
            {
                await Application.Current.MainPage.DisplayLocalizedAlert(
                    "SaveFailTitle", "FailMsg");
            }

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnCancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
