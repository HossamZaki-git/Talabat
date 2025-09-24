using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.AccountsVMs
{
    public class LoginVM
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
        public bool RememberTheLogin { get; set; }
    }
}
