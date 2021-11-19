using System.Collections.Generic;
using FoodRecipes.Core.Domain.Security;
using FoodRecipes.Core.Domain.Users;

namespace FoodRecipes.Services.Security
{
    public partial class StandardPermissionProvider : IPermissionProvider
    {
        //admin area permissions
        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
        public static readonly PermissionRecord ManageRecipes = new PermissionRecord { Name = "Admin area. Manage recipes", SystemName = "ManageRecipes", Category = "Recipe" };
        public static readonly PermissionRecord ManageCategories = new PermissionRecord { Name = "Admin area. Manage categories", SystemName = "ManageCategories", Category = "Recipe" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage users", SystemName = "ManageUsers", Category = "User" };

        //public permissions
        public static readonly PermissionRecord DisplayRecipes = new PermissionRecord { Name = "Display recipes", SystemName = "DisplayRecipes", Category = "Recipe" };

        public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            return new HashSet<(string, PermissionRecord[])>
            {
                (
                    FoodRecipesUserDefaults.AdministratorsRoleName,
                    new[]
                    {
                        AccessAdminPanel,
                        ManageRecipes,
                        ManageCategories,
                        ManageUsers
                    }
                ),
                (
                    FoodRecipesUserDefaults.RegisteredRoleName,
                    new[]
                    {
                        DisplayRecipes
                    }
                ),
                (
                    FoodRecipesUserDefaults.GuestsRoleName,
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
