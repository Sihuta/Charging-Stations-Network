using ChargingStationsApp.Localization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ChargingStationsApp.Extensions
{
    internal static class AlertExtensions
    {
        public static async Task DisplayLocalizedAlert(
            this Page currentPage, string titleKey, string msgKey, string btn = "OK")
        {
            string title = TranslateExtension.GetValue(titleKey);
            string msg = TranslateExtension.GetValue(msgKey);

            await currentPage.DisplayAlert(title, msg, btn);
        }
    }
}
