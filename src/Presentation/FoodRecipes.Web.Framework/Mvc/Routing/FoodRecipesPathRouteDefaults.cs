namespace FoodRecipes.Web.Framework.Mvc.Routing
{
    public static partial class FoodRecipesPathRouteDefaults
    {
        /// <summary>
        /// Gets default key for action field
        /// </summary>
        public static string ActionFieldKey => "action";

        /// <summary>
        /// Gets default key for controller field
        /// </summary>
        public static string ControllerFieldKey => "controller";

        /// <summary>
        /// Gets default key for permanent redirect field
        /// </summary>
        public static string PermanentRedirectFieldKey => "permanentRedirect";

        /// <summary>
        /// Gets default key for url field
        /// </summary>
        public static string UrlFieldKey => "url";

        /// <summary>
        /// Gets default key for category id field
        /// </summary>
        public static string CategoryIdFieldKey => "categoryid";

        /// <summary>
        /// Gets default key for product id field
        /// </summary>
        public static string RecipeIdFieldKey => "recipeid";

        /// <summary>
        /// Gets default key for se name field
        /// </summary>
        public static string SeNameFieldKey => "sename";

        /// <summary>
        /// Gets language route value
        /// </summary>
        public static string LanguageRouteValue => "language";

        /// <summary>
        /// Gets language parameter transformer
        /// </summary>
        public static string LanguageParameterTransformer => "lang";
    }
}