using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Media;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Media.Caching
{
    /// <summary>
    /// Represents a picture cache event consumer
    /// </summary>
    public partial class PictureCacheEventConsumer : CacheEventConsumer<Picture>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Picture entity)
        {
            await RemoveByPrefixAsync(FoodRecipesMediaDefaults.ThumbsExistsPrefix);
        }
    }
}
