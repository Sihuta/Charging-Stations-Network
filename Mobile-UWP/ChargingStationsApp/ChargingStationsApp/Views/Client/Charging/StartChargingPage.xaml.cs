﻿using ChargingStationsApp.ViewModels.Client.Charging;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChargingStationsApp.Views.Client.Charging
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartChargingPage : ContentPage
    {
        private readonly StartChargingViewModel viewModel;

        public StartChargingPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StartChargingViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearingAsync();
        }
    }
}