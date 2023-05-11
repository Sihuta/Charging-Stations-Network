using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ChargingStationsApp.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool ContainsIgnoreCase(this string str, string substr)
        {
            return str.IndexOf(substr, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
