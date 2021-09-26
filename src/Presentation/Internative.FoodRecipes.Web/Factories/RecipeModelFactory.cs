using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Web.Infrastructure.Mapper.Extensions;
using Internative.FoodRecipes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Factories
{
    public class RecipeModelFactory : IRecipeModelFactory
    {
        public async Task<RecipeModel> PrepareRecipeModelAsync(RecipeModel model, Recipe recipe)
        {
            if(recipe != null)
            {
                if(model == null)
                {
                    model = recipe.ToModel<RecipeModel>();
                }
            }

            return model;
        }
    }
}
