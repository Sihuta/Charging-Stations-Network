using ChargingStationsApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace ChargingStationsApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}