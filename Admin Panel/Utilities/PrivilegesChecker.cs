using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Identity;

namespace Admin_Panel.Utilities
{
    public static class PrivilegesChecker
    {
        public class PrivilegCheckResult
        {
            public PrivilegCheckResult(bool isAllowed, Privilege privilege)
            {
                this.isAllowed = isAllowed;
                string messagePrefix = "❌ You aren't allowed to";
                Message = privilege switch
                {
                    Privilege.ProductsCreation => $"{messagePrefix} create products",
                    Privilege.RolesCreation => $"{messagePrefix} craete roles",
                    Privilege.UsersCreation => $"{messagePrefix} create users",
                    Privilege.Products_EditDelete => $"{messagePrefix} edit/delete a product",
                    Privilege.Roles_EditDelete => $"{messagePrefix} edit/delete a role",
                    Privilege.Users_EditDelete => $"{messagePrefix} edit/delete a user"
                };
            }
            public string? Message { get; set; }
            public bool isAllowed { get; set; }
        }

        static async Task<Privilege[]> GetUserPrivileges(ClaimsPrincipal User, ApplicationIdentityContext identityContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            // The target user
            var user = await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (user is null)
                return null;
            // All the names of the roles that the user is involved in
            var userRolesNames = (await userManager.GetRolesAsync(user));
            // The ids of the user's roles
            IQueryable<string> rolesIDs = roleManager.Roles.Where(R => userRolesNames.Any(RN => RN == R.Name)).Select(R => R.Id);
            // The privileges of those roles
            var privileges = identityContext.RolesPrivileges.Where(
                                                                    RP => rolesIDs.Any(RID => RID == RP.RoleID)
                                                                    ).Select(RP => RP.privilege).Distinct();
            return privileges.ToArray();

        }

        // Checks if that privilege is one of the user's privilege
        public static async Task<PrivilegCheckResult> CheckAsync(HttpContext httpContext, Privilege privilege)
        {
            var User = httpContext.User;
            var userManager = httpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = httpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            var identityContext = httpContext.RequestServices.GetRequiredService<ApplicationIdentityContext>();

            var userPrivileges = await GetUserPrivileges(User: User, roleManager: roleManager, identityContext: identityContext, userManager: userManager);

            if (userPrivileges is null)
                return new PrivilegCheckResult(false, privilege);

            bool flag = false;
            int i = 0;
            while(i < userPrivileges.Length && !flag)
                flag = privilege == userPrivileges[i++];
            return new PrivilegCheckResult(flag, privilege);
        }
    }
}
