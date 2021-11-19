using FoodRecipes.Core.Domain.Security;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Security.Caching
{
    /// <summary>
    /// Represents a permission record-customer role mapping cache event consumer
    /// </summary>
    public partial class PermissionRecordCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<PermissionRecordUserRoleMapping>
    {
    }
}