using FoodRecipes.Core.Domain.ScheduleTasks;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.ScheduleTasks.Caching
{
    /// <summary>
    /// Represents a schedule task cache event consumer
    /// </summary>
    public partial class ScheduleTaskCacheEventConsumer : CacheEventConsumer<ScheduleTask>
    {
    }
}
