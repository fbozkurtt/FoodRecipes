using Internative.FoodRecipes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Common.Interfaces
{
    public interface IRecipeService
    {
        Task InsertRecipeAsync(Recipe recipe);

        Task InsertRecipeAsync(IList<Recipe> recipes);

        Task<Recipe> GetRecipeByIdAsync(string id);

        Task<IPagedList<Recipe>> GetAllRecipesPaginatedAsync(int pageIndex = 0, int pageSize = int.MaxValue);

        Task UpdateRecipeAsync(Recipe recipe);

        Task DeleteRecipeAsync(Recipe recipe);

        Task DeleteRecipeAsync(IList<Recipe> recipes);
    }
}
