using ChargingStationsApp.Enums;
using Xamarin.Forms.Maps;

namespace ChargingStationsApp.Models
{
    internal class MappedStation
    {
        public Position Position { get; set; }
        public string Name { get; set; }
        public ConnectorType ConnectorType { get; set; }
        public StationState State { get; set; }
    }
}
