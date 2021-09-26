using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Web.Factories;
using Internative.FoodRecipes.Web.Infrastructure.Mapper.Extensions;
using Internative.FoodRecipes.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Areas.Admin.Controllers
{
    public class RecipeController : BaseAdminController
    {
        #region Fields

        private readonly IRecipeService _recipeService;
        private readonly IRecipeModelFactory _recipeModelFactory;

        #endregion

        #region Ctor

        public RecipeController(IRecipeService recipeService, IRecipeModelFactory recipeModelFactory)
        {
            _recipeService = recipeService;
            _recipeModelFactory = recipeModelFactory;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> List()
        {
            var recipes = (await _recipeService.GetAllRecipesPaginatedAsync()).Select(_ => _.ToModel<RecipeModel>());
            return View(recipes);
        }

        public virtual async Task<IActionResult> Details(string id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            var model = await _recipeModelFactory.PrepareRecipeModelAsync(null, recipe);


            if (model != null)
                return View(model);

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
