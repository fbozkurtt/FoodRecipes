using Internative.FoodRecipes.Application.Security;
using Internative.FoodRecipes.Domain.Entities;
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
        public static readonly PermissionRecord ManageRecipes = new PermissionRecord { Name = "Admin area. Manage recipes", SystemName = "ManageCategories", Category = "Catalog" };
        public static readonly PermissionRecord ManageUsers = new PermissionRecord { Name = "Admin area. Manage recipes", SystemName = "ManageCategories", Category = "Catalog" };
        public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
