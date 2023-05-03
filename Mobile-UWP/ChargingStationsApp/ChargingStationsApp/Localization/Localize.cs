using System.Globalization;

namespace ChargingStationsApp.Localization
{
    internal class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            string lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return new CultureInfo(lang);
        }
    }
}
