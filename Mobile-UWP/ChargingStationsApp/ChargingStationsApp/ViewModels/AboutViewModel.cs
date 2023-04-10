﻿using System;
using System.Windows.Input;
using ChargingStationsApp.ViewModels.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ChargingStationsApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}