namespace FoodRecipes.Core.Domain.Recipes
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class RecipePicture : BaseEntity
    {
        /// <summary>
        /// Gets or sets the recipe identifier
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
