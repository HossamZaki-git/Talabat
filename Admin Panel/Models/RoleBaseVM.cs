using Admin_Panel.Utilities;
using Talabat.Core.Domain_Models.Identity;

namespace Admin_Panel.Models
{
    public class RoleBaseVM
    {
        public List<Pair<Privilege, bool>> Privileges { get; set; } = new List<Pair<Privilege, bool>>();
    }
}
