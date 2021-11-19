using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Users.Caching
{
    /// <summary>
    /// Represents a customer customer role mapping cache event consumer
    /// </summary>
    public partial class UserUserRoleMappingCacheEventConsumer : CacheEventConsumer<UserUserRoleMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(UserUserRoleMapping entity)
        {
            await RemoveByPrefixAsync(FoodRecipesUserServicesDefaults.UserUserRolesPrefix);
        }
    }
}