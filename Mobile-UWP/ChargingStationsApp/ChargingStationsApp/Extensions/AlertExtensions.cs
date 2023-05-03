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

        public static async Task DisplayLocalizedAlert(
            this Page currentPage, string titleKey, string msgKey, object obj, string btn = "OK")
        {
            string title = TranslateExtension.GetValue(titleKey);
            string msg = TranslateExtension.GetValue(msgKey);

            await currentPage.DisplayAlert(title, $"{msg} {obj}", btn);
        }

        public static async Task<bool> DisplayLocalizedAlertWithChoice(
            this Page currentPage, string titleKey, string msgKey, string yesBtnKey = "YesBtn", string noBtnKey = "NoBtn")
        {
            string title = TranslateExtension.GetValue(titleKey);
            string msg = TranslateExtension.GetValue(msgKey);
            string yes = TranslateExtension.GetValue(yesBtnKey);
            string no = TranslateExtension.GetValue(noBtnKey);

            return await currentPage.DisplayAlert(title, msg, yes, no);
        }
    }
}
