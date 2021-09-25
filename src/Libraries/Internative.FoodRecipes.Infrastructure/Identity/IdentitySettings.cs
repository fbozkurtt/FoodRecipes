using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Identity
{
    public class IdentitySettings
    {
        public static PasswordOptions GetPasswordOptions()
        {
            return new PasswordOptions()
            {
                RequiredLength = 5,
                RequireDigit = false,
                RequireUppercase = false,
                RequireLowercase = false,
                RequireNonAlphanumeric = false,
                RequiredUniqueChars = 0,
            };
        }

        public static IList<IdentityRole<int>> GetDefaultRoles()
        {
            var roles = new List<IdentityRole<int>>()
            {
                { new IdentityRole<int>(RoleNames.Admin) },
                { new IdentityRole<int>(RoleNames.Moderator) },
                { new IdentityRole<int>(RoleNames.User) },
            };
            return roles;
        }
    }
}
