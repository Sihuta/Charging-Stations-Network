using ChargingStationsApp.Models;
using ChargingStationsApp.Services.Interfaces;
using ChargingStationsApp.ViewModels.Shared;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using System.Linq;
using ChargingStationsApp.Extensions;
using ChargingStationsApp.Views.Admin.Admins;

namespace ChargingStationsApp.ViewModels.Admin.Admins
{
    internal class AdminsViewModel : BaseViewModel
    {
        private readonly IUserService userService;

        public AdminsViewModel()
        {
            userService = DependencyService.Get<IUserService>();

            Admins = new ObservableCollection<User>();

            LoadAdminsCommand = new Command(
                async () => await LoadAdminsAsync());
            AddAdminCommand = new Command(
                async () => await OnAddAdminAsync());
            RemoveAdmin = new Command<int>(
                async (id) => await OnRemoveAdminAsync(id));
        }

        public ObservableCollection<User> Admins { get; }
        public Command LoadAdminsCommand { get; }
        public Command AddAdminCommand { get; }
        public Command<int> RemoveAdmin { get; }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async Task LoadAdminsAsync()
        {
            IsBusy = true;

            try
            {
                Admins.Clear();
                var users = await userService.GetUsersAsync();
                foreach (var admin in users
                    .Where(u => u.Role == userService.AdminRole))
                {
                    Admins.Add(admin);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnAddAdminAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddAdminPage));
        }

        private async Task OnRemoveAdminAsync(int id)
        {
            if (await Application.Current.MainPage
                .DisplayLocalizedAlertWithChoice("ConfirmRemoveTitle", "ConfirmRemoveMsg"))
            {
                await userService.DeleteUserAsync(id);
                IsBusy = true;
            }
        }
    }
}
