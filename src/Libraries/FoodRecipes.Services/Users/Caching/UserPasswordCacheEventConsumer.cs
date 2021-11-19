using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Services.Caching;

namespace FoodRecipes.Services.Users.Caching
{
    /// <summary>
    /// Represents a customer password cache event consumer
    /// </summary>
    public partial class UserPasswordCacheEventConsumer : CacheEventConsumer<UserPassword>
    {
    }
}