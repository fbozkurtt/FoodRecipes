using FoodRecipes.Core.Caching;
using FoodRecipes.Core.Domain.Configuration;

namespace FoodRecipes.Services.Configuration
{
    /// <summary>
    /// Represents default values related to settings
    /// </summary>
    public static partial class FoodRecipesSettingsDefaults
    {
        #region Caching defaults

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new CacheKey("Nop.setting.all.dictionary.", FoodRecipesEntityCacheDefaults<Setting>.Prefix);

        #endregion
    }
}