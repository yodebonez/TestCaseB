using System.ComponentModel.DataAnnotations;

namespace TestCaseB.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerNumber { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ShipingIdentity { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string ShippingStatus { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public DateTime DateShipped { get; set; }
        public double Distance { get; set; }
        public double GoodsZise { get; set; }
        public decimal ShippingCost { get; set; }


     
    }
}
