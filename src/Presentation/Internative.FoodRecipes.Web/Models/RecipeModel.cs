using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Models
{
    public partial record RecipeModel : BaseEntityModel
    {
        public RecipeModel()
        {
            Ingredients = new List<Ingredient>();
        }
        public string Title { get; set; }
        public IList<string> Instructions { get; set; }
        public IList<Ingredient> Ingredients { get; set; }
        public int? CookingTime { get; set; }
        public int? PreparingTime { get; set; }
        public int TotalViews { get; set; }
        public int[] PictureIds { get; set; }
        public int ViewCount { get; set; }

        #region Nested Classes

        public class Ingredient
        {
            public string Name { get; set; }
            public string Amount { get; set; }
            public string Unit { get; set; }
        }

        #endregion
    }
}
