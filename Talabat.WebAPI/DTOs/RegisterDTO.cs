using System.ComponentModel.DataAnnotations;

namespace Talabat.WebAPI.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}", 
            ErrorMessage = "The password must be at least 8 chars, containing at least one of each of the following uppercase, lowercase, digit & special character")]
        public string Password { get; set; }
    }
}
