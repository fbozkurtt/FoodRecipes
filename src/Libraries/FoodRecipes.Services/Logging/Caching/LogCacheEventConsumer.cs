using FoodRecipes.Core.Domain.Logging;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Logging.Caching
{
    /// <summary>
    /// Represents a log cache event consumer
    /// </summary>
    public partial class LogCacheEventConsumer : CacheEventConsumer<Log>
    {
    }
}
