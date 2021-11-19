using FoodRecipes.Core.Caching;
using FoodRecipes.Core.Domain.Directory;

namespace FoodRecipes.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class FoodRecipesDirectoryDefaults
    {
        #region Caching defaults

        #region Countries

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : Two letter ISO code
        /// </remarks>
        public static CacheKey CountriesByTwoLetterCodeCacheKey => new CacheKey("FoodRecipes.country.bytwoletter.{0}", FoodRecipesEntityCacheDefaults<Country>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : Two letter ISO code
        /// </remarks>
        public static CacheKey CountriesByThreeLetterCodeCacheKey => new CacheKey("FoodRecipes.country.bythreeletter.{0}", FoodRecipesEntityCacheDefaults<Country>.Prefix);

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// {2} : current store ID
        /// </remarks>
        public static CacheKey CountriesAllCacheKey => new CacheKey("FoodRecipes.country.all.{0}-{1}-{2}", FoodRecipesEntityCacheDefaults<Country>.Prefix);

        #endregion

        #endregion
    }
}
