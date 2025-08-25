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
        public static async Task Seed(UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any())
                return;

            await userManager.CreateAsync(new ApplicationUser
            {
                DispalyName = "Hossam Zaki",
                UserName = "ha307122",
                Email = "ha307122@gmail.com",
                PhoneNumber = "01000000000"
            }, "Pa$$w0rd");
        }
    }
}
