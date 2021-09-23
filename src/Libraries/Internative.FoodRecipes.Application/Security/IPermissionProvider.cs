using Internative.FoodRecipes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Security
{
    /// <summary>
    /// Permission provider
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// Get permissions
        /// </summary>
        /// <returns>Permissions</returns>
        IEnumerable<PermissionRecord> GetPermissions();

        /// <summary>
        /// Get default permissions
        /// </summary>
        /// <returns>Default permissions</returns>
        HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions();
    }
}
