using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Identity;

namespace Talabat.Repository.Identity
{
    public static class IdentitySeeder
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationIdentityContext identityContext)
        {
            if (userManager.Users.Any())
                return;

            await userManager.CreateAsync(new ApplicationUser
            {
                Email = "admin000@talabat.com",
                UserName = "admin",
                DispalyName = "Admin"
            },
            "Pa$$w0rd");

            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "admin"
            });

            var admin = userManager.Users.FirstOrDefault();
            var role = roleManager.Roles.FirstOrDefault();


            await userManager.AddToRoleAsync(admin, role.Name);

            // Adding all the privileges to the admin role
            foreach (Privilege p in Enum.GetValues(typeof(Privilege)))
                identityContext.Add(new RolePrivilege
                {
                    privilege = p,
                    Role = role
                });

            identityContext.SaveChanges();
        }
    }
}
