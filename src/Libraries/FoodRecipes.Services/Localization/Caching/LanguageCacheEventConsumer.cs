using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Localization;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Localization.Caching
{
    /// <summary>
    /// Represents a language cache event consumer
    /// </summary>
    public partial class LanguageCacheEventConsumer : CacheEventConsumer<Language>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Language entity)
        {
            await RemoveAsync(FoodRecipesLocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, entity);
            await RemoveAsync(FoodRecipesLocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, entity);
            await RemoveAsync(FoodRecipesLocalizationDefaults.LocaleStringResourcesAllCacheKey, entity);
            await RemoveByPrefixAsync(FoodRecipesLocalizationDefaults.LocaleStringResourcesByNamePrefix, entity);
        }
    }
}