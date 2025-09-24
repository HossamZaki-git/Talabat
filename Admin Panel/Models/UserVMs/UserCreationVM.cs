using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.UserVMs
{
    public class UserCreationVM : UserFormBaseVM
    {

        [Required]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}",
            ErrorMessage = "The password must be at least 8 chars, containing at least one of each of the following uppercase, lowercase, digit & special character")]
        public string Password { get; set; }
    }
}
