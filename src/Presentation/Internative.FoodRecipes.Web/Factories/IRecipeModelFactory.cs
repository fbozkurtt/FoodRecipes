using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Factories
{
    public partial interface IRecipeModelFactory
    {
        Task<RecipeModel> PrepareRecipeModelAsync(RecipeModel model, Recipe recipe);
    }
}
