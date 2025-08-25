using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string DispalyName { get; set; }
        public Address Address { get; set; }
    }
}
