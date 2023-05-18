using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ChargingStationsApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init("02bMGt1gHm22UGMAzIdQ~eHsQBI-vHIZVh8aXe67w9w~AgJxg2-QEuUZCyb0Sc4Eroc3bdZP9HPrfiaYKgImAb_3311GdHkkWiVczPdCGVaZ");
            LoadApplication(new ChargingStationsApp.App());
        }
    }
}
