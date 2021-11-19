namespace FoodRecipes.Web.Framework.Models
{
    /// <summary>
    /// Alert model
    /// </summary>
    public record ActionAlertModel : BaseFoodRecipesEntityModel
    {
        /// <summary>
        /// Window ID
        /// </summary>
        public string WindowId { get; set; }
        /// <summary>
        /// Alert ID
        /// </summary>
        public string AlertId { get; set; }
        /// <summary>
        /// Alert message
        /// </summary>
        public string AlertMessage { get; set; }
    }
}
