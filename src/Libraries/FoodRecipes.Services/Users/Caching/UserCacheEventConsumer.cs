using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Services.Caching;
using FoodRecipes.Services.Events;

namespace FoodRecipes.Services.Users.Caching
{
    /// <summary>
    /// Represents a customer cache event consumer
    /// </summary>
    public partial class UserCacheEventConsumer : CacheEventConsumer<User>, IConsumer<UserPasswordChangedEvent>
    {
        #region Methods

        /// <summary>
        /// Handle password changed event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task HandleEventAsync(UserPasswordChangedEvent eventMessage)
        {
            await RemoveAsync(FoodRecipesUserServicesDefaults.UserPasswordLifetimeCacheKey, eventMessage.Password.UserId);
        }
 
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(User entity)
        {
            await RemoveByPrefixAsync(FoodRecipesUserServicesDefaults.UserUserRolesByUserPrefix, entity);
            await RemoveAsync(FoodRecipesUserServicesDefaults.UserByGuidCacheKey, entity.UserGuid);

            if (string.IsNullOrEmpty(entity.SystemName))
                return;

            await RemoveAsync(FoodRecipesUserServicesDefaults.UserBySystemNameCacheKey, entity.SystemName);
        }

        #endregion
    }
}