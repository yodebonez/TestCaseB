namespace TestCaseB.Models
{
    public class ShippingUpdate
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string? CurrentLocation { get; set; }
        public DateTime DateTime { get; set; }

        public string ShipingIdentity { get; set; }

    }
}
