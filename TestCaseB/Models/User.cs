using System.ComponentModel.DataAnnotations;

namespace TestCaseB.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string CustomerNumber { get; set; }

        public DateTime DateRegisted { get; set; }

        public string ActivationCode { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
