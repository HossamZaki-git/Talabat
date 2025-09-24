using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Identity;

namespace Talabat.Repository.Identity
{
    public class ApplicationIdentityContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<RolePrivilege> RolesPrivileges { get; set; }
        public ApplicationIdentityContext(DbContextOptions<ApplicationIdentityContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RolePrivilege>().HasKey(nameof(RolePrivilege.privilege), nameof(RolePrivilege.RoleID));
            builder.Entity<RolePrivilege>().Ignore("Id");
            base.OnModelCreating(builder);
        }
    }
}
