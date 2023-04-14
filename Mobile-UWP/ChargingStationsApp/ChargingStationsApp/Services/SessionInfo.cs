using ChargingStationsApp.Models;

namespace ChargingStationsApp.Services
{
    internal static class SessionInfo
    {
        public static bool IsAdmin { get => User.Role == "admin"; }
        public static bool IsClient { get => !IsAdmin; }
        public static User User { get; set; }
    }
}
