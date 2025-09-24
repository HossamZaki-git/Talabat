using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models
{
    public class RoleEdit_VM : RoleBaseVM
    {
        public string OldName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role name can't be empty")]
        [RegularExpression(pattern: @"\S+", ErrorMessage = "The role name can't be a white space")]
        public string NewName { get; set; }
    }
}
