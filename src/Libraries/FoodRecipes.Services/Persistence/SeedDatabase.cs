using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodRecipes.Core.Domain.Recipes;

namespace FoodRecipes.Services.Persistence
{
    public static class SeedDatabase
    {
        // public static async Task SeedSampleDataAsync(IRepository<Recipe> recipeRepository)
        // {
        //     IList<Recipe> recipes = new List<Recipe>()
        //     {
        //         new Recipe(){
        //             CookingTime=10,
        //             PreparingTime=15,
        //             Title="Crispy Cider-Battered Chicken Fingers",
        //             Ingredients = new List<Recipe.Ingredient>()
        //             {
        //                 new Recipe.Ingredient() { Name = "skinless, boneless chicken breast halves", Amount = "8", Unit = "ounce(s)" },
        //                 new Recipe.Ingredient() { Name = "kosher salt", Amount = "1", Unit = "teaspoon(s)" },
        //                 new Recipe.Ingredient() { Name = "freshly ground black pepper", Amount = "1/4", Unit = "teaspoon(s)" },
        //                 new Recipe.Ingredient() { Name = "cayenne pepper", Amount = "1", Unit = "pinch(s)" },
        //                 new Recipe.Ingredient() { Name = "all-purpose flour", Amount = "2", Unit = "tablespoon(s)" },
        //                 new Recipe.Ingredient() { Name = "self-rising flour", Amount = "1", Unit = "cup(s)" },
        //                 new Recipe.Ingredient() { Name = "sparkling apple cider", Amount = "1 1/4", Unit = "cup(s)" },
        //                 new Recipe.Ingredient() { Name = "canola oil for frying", Amount = "1", Unit = "quart" },
        //             },
        //             Instructions = new List<string>()
        //             {
        //                 "Cut each chicken breast into 6 finger-sized strips, and transfer into a mixing bowl.",
        //                 "Season with kosher salt, black pepper, and cayenne and toss until evenly coated. Sprinkle the all-purpose flour over top and shake the bowl to coat the strips. Toss until all the surfaces are completely coated, and then transfer onto a rack or plate. Place in the refrigerator, uncovered, for at least 10 minutes, or until ready to fry.",
        //                 "Meanwhile, whisk self-rising flour and sparkling apple cider together in a mixing bowl to form a thin batter. Batter should be somewhere between a thin pancake batter and a crepe batter, but still thick enough to coat the back of a spoon. Place in the refrigerator to rest for 5 to 10 minutes before using.",
        //                 "Mix Dijon mustard, vinegar, hot sauce, and sugar together for dipping sauce in a bowl.",
        //                 "Heat oil in a deep-fryer or large saucepan to 375 degrees F (190 degrees C).",
        //                 "Dip each chicken finger into the batter to coat. Deep-fry in batches in the hot oil until golden brown, crispy, and no longer pink in the centers, 3 to 4 1/2 minutes. Drain on paper towels and serve immediately with dipping sauce.",
        //             },
        //         },
        //     };
        //
        //     var recipesInDb = await recipeRepository.GetAllAsync(query =>
        //     {
        //         return query;
        //     });
        //
        //     recipes = recipes.Where(_ => !recipesInDb.Any(d => d.Title.Equals(_.Title))).ToList();
        //
        //     if (recipes.Count > 0)
        //         await recipeRepository.InsertAsync(recipes);
        // }
    }
}
