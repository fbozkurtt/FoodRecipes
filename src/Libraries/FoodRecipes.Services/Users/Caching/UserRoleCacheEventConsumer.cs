using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Users.Caching
{
    /// <summary>
    /// Represents a customer role cache event consumer
    /// </summary>
    public partial class UserRoleCacheEventConsumer : CacheEventConsumer<UserRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserRole entity)
        {
            await RemoveByPrefixAsync(FoodRecipesUserServicesDefaults.UserRolesBySystemNamePrefix);
            await RemoveByPrefixAsync(FoodRecipesUserServicesDefaults.UserUserRolesPrefix);
        }
    }
}
