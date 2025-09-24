using Admin_Panel.Utilities;
using System.ComponentModel.DataAnnotations;
using Talabat.Core.Domain_Models.Identity;

namespace Admin_Panel.Models
{
    public class RoleCreation_VM : RoleBaseVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Role name can't be empty")]
        [RegularExpression(pattern: @"\S+", ErrorMessage = "The role name can't be a white space")]
        public string RoleName { get; set; }
    }
}
