using Internative.FoodRecipes.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Entities
{
    public partial class PermissionRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Gets or sets the permission system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the permission category
        /// </summary>
        public string Category { get; set; }
    }
}
