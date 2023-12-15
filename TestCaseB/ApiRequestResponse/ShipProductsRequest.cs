using System.ComponentModel.DataAnnotations;

namespace TestCaseB.ApiRequestResponse
{
    public class ShipProductsRequest
    {
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string ToAddress { get; set; }
        public double GoodSizeinKg { get; set; }
        [Required]
        public string Country {  get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string ProductName { get; set; }

    }
}
