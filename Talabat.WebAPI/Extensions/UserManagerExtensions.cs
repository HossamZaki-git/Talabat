using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Identity;

namespace Talabat.WebAPI.Extensions
{
    public static class UserManagerExtensions
    {
        public static ApplicationUser FindUser_AddressInclusion(this UserManager<ApplicationUser> userManager, ClaimsPrincipal User)
            => userManager.Users.Where(U => U.Email == User.FindFirstValue(ClaimTypes.Email)).Include(U => U.Address).FirstOrDefault();

    }
}
