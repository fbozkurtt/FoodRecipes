using FoodRecipes.Core.Domain.Logging;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Logging.Caching
{
    /// <summary>
    /// Represents an activity log cache event consumer
    /// </summary>
    public partial class ActivityLogCacheEventConsumer : CacheEventConsumer<ActivityLog>
    {
    }
}