using System;

namespace ChargingStationsApp.Models
{
    class ConnectorType : IEquatable<ConnectorType>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxPower { get; set; }

        public bool Equals(ConnectorType other)
        {
            if (other is null) return false;
            return Name.Equals(other.Name);
        }
    }
}
