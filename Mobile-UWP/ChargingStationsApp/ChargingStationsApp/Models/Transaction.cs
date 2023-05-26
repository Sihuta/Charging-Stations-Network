using System;

namespace ChargingStationsApp.Models
{
    internal class Transaction
    {
        public int Id { get; set; }
        public Session Session { get; set; }
        public Tariff Tariff { get; set; }
        public double RequestedEnergy { get; set; }
        public double ChargedEnergy { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public decimal Pay { get => Tariff.Price * (decimal) ChargedEnergy; }
    }
}
