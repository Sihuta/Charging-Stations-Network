namespace ChargingStationsApp.Models
{
    internal class Session
    {
        public int Id { get; set; }
        public int PaymentCardId { get; set; }
        public Station Station { get; set; }
    }
}
