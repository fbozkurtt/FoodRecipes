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
        public PasswordOptions GetPasswordOptions()
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

        public static IList<IdentityRole<Guid>> GetDefaultRoles()
        {
            var roles = new List<IdentityRole<Guid>>()
            {
                { new IdentityRole<Guid>(DefaultRoleNames.Admin) },
                { new IdentityRole<Guid>(DefaultRoleNames.User) },
            };
            return roles;
        }
    }
}
