using System.Globalization;
using System.Reflection;
using System.Resources;
using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace ChargingStationsApp.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "ChargingStationsApp.Resources.Lang";

        public TranslateExtension()
        {
            ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var resmgr = new ResourceManager(ResourceId,
                        typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);
            if (translation == null)
            {
                translation = Text;
            }

            return translation;
        }

        public static string GetValue(string key)
        {
            var resmgr = new ResourceManager(
                ResourceId,
                typeof(TranslateExtension).GetTypeInfo().Assembly);

            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            var translation = resmgr.GetString(key, ci);
            if (translation == null)
            {
                translation = key;
            }

            return translation;
        }
    }
}
