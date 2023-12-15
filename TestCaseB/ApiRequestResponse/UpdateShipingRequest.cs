namespace TestCaseB.ApiRequestResponse
{
    public class UpdateShipingRequest
    {
        public string CustomerEmail { get; set; }
        public string ShipingId { get; set; }
        public string ShippingStatus { get; set; }
        public string CurrentLocation { get; set; }
    }
}
