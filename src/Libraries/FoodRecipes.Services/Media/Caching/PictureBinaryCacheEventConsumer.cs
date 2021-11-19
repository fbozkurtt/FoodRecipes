using FoodRecipes.Core.Domain.Media;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Media.Caching
{
    /// <summary>
    /// Represents a picture binary cache event consumer
    /// </summary>
    public partial class PictureBinaryCacheEventConsumer : CacheEventConsumer<PictureBinary>
    {
    }
}
