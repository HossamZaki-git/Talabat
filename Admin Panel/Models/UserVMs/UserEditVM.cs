using Admin_Panel.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Admin_Panel.Models.UserVMs
{
    public class UserEditVM : UserFormBaseVM
    {
        public string ID { get; set; }
        [AllowNull]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}",
    ErrorMessage = "The password must be at least 8 chars, containing at least one of each of the following uppercase, lowercase, digit & special character")]
        public string? Password { get; set; }

        public List<Pair<string, bool>> isInRole { get; set; } = new List<Pair<string, bool>>();
    }
}
