using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Security;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Security.Caching
{
    /// <summary>
    /// Represents a ACL record cache event consumer
    /// </summary>
    public partial class AclRecordCacheEventConsumer : CacheEventConsumer<AclRecord>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(AclRecord entity)
        {
            await RemoveAsync(FoodRecipesSecurityDefaults.AclRecordCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(FoodRecipesSecurityDefaults.EntityAclRecordExistsCacheKey, entity.EntityName);
        }
    }
}
