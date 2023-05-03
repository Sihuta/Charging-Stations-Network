using ChargingStationsApp.Extensions;
using ChargingStationsApp.Services;
using ChargingStationsApp.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels.Shared.Profile
{
    class ChangePasswordViewModel : BaseViewModel
    {
        private string oldPassword;
        private string newPassword;
        private string confirmNewPassword;

        private readonly IUserService userService;

        public ChangePasswordViewModel()
        {
            userService = DependencyService.Get<IUserService>();

            ConfirmCommand = new Command(OnConfirmClicked, ValidateChangePassword);
            PropertyChanged +=
                (_, __) => ConfirmCommand.ChangeCanExecute();
        }

        public string OldPassword
        {
            get => oldPassword;
            set => SetProperty(ref oldPassword, value);
        }

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        public string ConfirmNewPassword
        {
            get => confirmNewPassword;
            set => SetProperty(ref confirmNewPassword, value);
        }

        public Command ConfirmCommand { get; }

        private bool ValidateChangePassword(object _)
        {
            return !string.IsNullOrWhiteSpace(oldPassword)
                && !string.IsNullOrWhiteSpace(newPassword)
                && !string.IsNullOrWhiteSpace(confirmNewPassword);
        }

        private async void OnConfirmClicked(object _)
        {
            if (await ValidateWithFeedbackAsync())
            {
                SessionInfo.User.Password = newPassword;

                var updated = await userService.UpdateUserAsync(SessionInfo.User);
                if (updated)
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
        }

        private async Task<bool> ValidateWithFeedbackAsync()
        {
            bool isValid = true;
            string errorTitleKey = "InvalidInputTitle";
            string errorMsgKey = string.Empty;

            if (!SessionInfo.User.Password.Equals(oldPassword))
            {
                isValid = false;
                errorMsgKey = "InvalidInputOldPassMsg";
            }
            else if (oldPassword.Equals(newPassword))
            {
                isValid = false;
                errorMsgKey = "InvalidInputSamePassMsg";
            }
            else if (!confirmNewPassword.Equals(newPassword))
            {
                isValid = false;
                errorMsgKey = "InvalidInputNewPassMsg";
            }

            if (isValid)
            {
                return true;
            }

            await Application.Current.MainPage
                .DisplayLocalizedAlert(errorTitleKey, errorMsgKey);
            return false;
        }
    }
}
