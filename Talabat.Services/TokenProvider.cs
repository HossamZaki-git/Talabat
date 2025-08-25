using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration Configuration;

        public TokenProvider(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public async Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // comprises the secret that will be used in the signature
            SymmetricSecurityKey secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
            // comprises the secret & the name of the hashing algorithm
            SigningCredentials signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            // The private claims (the custom claims)
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DispalyName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Configuration["JWT:Issuer"],
                audience: Configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours( double.Parse(Configuration["JWT:Duration"]) ),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
