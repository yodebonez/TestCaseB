using System.ComponentModel.DataAnnotations;

namespace TestCaseB.ApiRequestResponse
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
