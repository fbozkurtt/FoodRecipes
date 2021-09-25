using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Services
{
    public class RecipeService : IRecipeService
    {
        #region Fields

        private readonly IRepository<Recipe> _recipeRepository;

        #endregion

        #region Ctor

        public RecipeService(IRepository<Recipe> recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        #endregion

        #region Methods

        public async Task DeleteRecipeAsync(Recipe recipe)
        {
            await _recipeRepository.DeleteAsync(recipe);
        }

        public async Task DeleteRecipeAsync(IList<Recipe> recipes)
        {
            await _recipeRepository.DeleteAsync(recipes);
        }

        public async Task<IPagedList<Recipe>> GetAllRecipesPaginatedAsync(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await _recipeRepository.GetAllPagedAsync(query => {
                return query.OrderBy(_ => _.Id);
            }, pageIndex: pageIndex, pageSize: pageSize);
        }

        public async Task<Recipe> GetRecipeByIdAsync(string id)
        {
            return await _recipeRepository.GetByIdAsync(id);
        }

        public async Task InsertRecipeAsync(Recipe recipe)
        {
            await _recipeRepository.InsertAsync(recipe);
        }

        public async Task InsertRecipeAsync(IList<Recipe> recipes)
        {
            await _recipeRepository.InsertAsync(recipes);
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            await _recipeRepository.UpdateAsync(recipe);
        }

        #endregion
    }
}
