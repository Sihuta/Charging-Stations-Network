using ChargingStationsApp.Enums;

namespace ChargingStationsApp.Models
{
    class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ServerUrl { get; set; }
        public string WifiSsid { get; set; }
        public string WifiPwd { get; set; }
        public ConnectorType ConnectorType { get; set; }
        public StationState State { get; set; }
    }
}
