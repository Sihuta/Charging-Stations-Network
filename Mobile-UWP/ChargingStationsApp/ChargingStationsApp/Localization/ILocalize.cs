using System.Globalization;

namespace ChargingStationsApp.Localization
{
    internal interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
    }
}
