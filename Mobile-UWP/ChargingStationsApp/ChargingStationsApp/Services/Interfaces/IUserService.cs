﻿using ChargingStationsApp.Models;
using System.Threading.Tasks;

namespace ChargingStationsApp.Services.Interfaces
{
    internal interface IUserService
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> LoadUserAsync(int id);
        Task<bool> UpdateUserAsync(User user);
    }
}
