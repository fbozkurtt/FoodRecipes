using FoodRecipes.Core.Domain.Media;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Media.Caching
{
    /// <summary>
    /// Represents a download cache event consumer
    /// </summary>
    public partial class DownloadCacheEventConsumer : CacheEventConsumer<Download>
    {
    }
}
