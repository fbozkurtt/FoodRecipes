using FoodRecipes.Core.Domain.Logging;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Logging.Caching
{
    /// <summary>
    /// Represents a activity log type cache event consumer
    /// </summary>
    public partial class ActivityLogTypeCacheEventConsumer : CacheEventConsumer<ActivityLogType>
    {
    }
}
