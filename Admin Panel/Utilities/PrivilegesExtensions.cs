using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Talabat.Core.Domain_Models.Identity;

namespace Admin_Panel.Utilities
{
    public static class PrivilegesExtensions
    {
        public static string GetDisplayName(this Privilege privilge)
        {
            return privilge.GetType()
                            .GetMember(privilge.ToString()) // Returns IEnumerable<MemberInfo>, each object of it represents all the data about every memeber with the given name
                            .First() // The first and only existing MemberInfo (The enum's members names are distincit)
                            .GetCustomAttribute<DisplayAttribute>()? // The value given to that property in the [Display] attribute
                            .Name ?? privilge.ToString();
        }
    }
}
