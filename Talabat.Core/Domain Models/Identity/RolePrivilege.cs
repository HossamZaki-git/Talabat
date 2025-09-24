using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Identity
{
    public enum Privilege
    {
        [Display(Name = "Role Creation")]
        RolesCreation,
        [Display(Name = "User Creation")]
        UsersCreation,
        [Display(Name = "Products Creation")]
        ProductsCreation,
        [Display(Name = "Deleting Roles")]
        Roles_EditDelete,
        [Display(Name = "Editing & Deleting Users")]
        Users_EditDelete,
        [Display(Name = "Editing & Deleting Products")]
        Products_EditDelete
    }
    public class RolePrivilege
    {
        public Privilege privilege { get; set; }
        [ForeignKey(nameof(Role))]
        public string RoleID { get; set; }
        public IdentityRole Role { get; set; }
    }
}
