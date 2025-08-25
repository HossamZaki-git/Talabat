using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Identity;

namespace Talabat.Core.Services
{
    public interface ITokenProvider
    {
        public Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
