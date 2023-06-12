using ChargingStationsApp.Models;

namespace ChargingStationsApp.Services
{
    internal static class SessionInfo
    {
        public static bool IsAdmin { get => User.Role == "admin"; }
        public static bool IsClient { get => User.Role == "client"; }
        public static bool IsGuest { get => User is null; }

        public static User User { get; set; }
        public static Transaction LastTransaction { get; set; }

        public static SearchOptions TransactionSearchOptions = new SearchOptions();

        public static void Logout()
        {
            User = null;
            LastTransaction = null;
            TransactionSearchOptions = new SearchOptions();
        }
    }
}
