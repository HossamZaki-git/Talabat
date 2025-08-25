using Microsoft.AspNetCore.Identity;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository.Identity;

namespace Talabat.WebAPI.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityContext>();

            return Services;
        }
    }
}
