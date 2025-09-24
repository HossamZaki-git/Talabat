using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.UserVMs
{
    public class UserFormBaseVM
    {
        [Required]
        [RegularExpression(pattern: @"[A-Za-z]+")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(pattern: @"[A-Za-z]+")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        public string Email { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [RegularExpression(pattern: @"[A-Za-z]+")]
        public string Country { get; set; }
    }
}
