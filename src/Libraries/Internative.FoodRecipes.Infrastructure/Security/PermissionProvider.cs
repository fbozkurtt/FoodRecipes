using Internative.FoodRecipes.Application.Security;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Security
{
    public partial class PermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord ManageRecipes = new PermissionRecord { Name = "Admin area. Manage recipes", SystemName = "ManageRecipes", Category = "Recipe" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage users", SystemName = "ManageUsers", Category = "User" };

        //public permissions
        public static readonly PermissionRecord DisplayRecipes = new PermissionRecord { Name = "Display recipes", SystemName = "DisplayRecipes", Category = "Recipe" };

        public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    RoleNames.Admin,
                    new[]
                    {
                        AccessAdminPanel,
                        ManageRecipes,
                        ManageUsers
                    }
                ),
                (
                    RoleNames.Moderator,
                    new[]
                    {
                        AccessAdminPanel,
                        ManageRecipes,
                    }
                ),
                (
                    RoleNames.User,
                    new[]
                    {
                        DisplayRecipes
                    }
                )
            };
        }

        public virtual IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel,
                ManageRecipes,
                ManageUsers,
                DisplayRecipes
            };

        }
    }
}
