using FoodRecipes.Core.Configuration;

namespace FoodRecipes.Core.Security
{
    public partial class CookieSettings : ISettings
    {

        /// <summary>
        /// Expiration time on hours for the "Recently viewed recipes" cookie
        /// </summary>
        public int RecentlyViewedRecipesCookieExpires { get; set; }

        /// <summary>
        /// Expiration time on hours for the "User" cookie
        /// </summary>
        public int UserCookieExpires { get; set; }
    }
}
