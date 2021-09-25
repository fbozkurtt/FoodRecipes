using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IdentityUser<int>> GetUserAsync(int userId);

        Task<IdentityUser<int>> GetUserAsync(string username);

        Task<string> GetUserNameAsync(int userId);

        Task<bool> IsInRoleAsync(int userId, string role);

        Task<bool> AuthorizeAsync(int userId, string policyName);

        Task<int> CreateUserAsync(string username, string password);

        Task<bool> DeleteUserAsync(int userId);

        Task<bool> DeleteUserAsync(string username);

        Task<SecurityToken> GetTokenAsync(string username, string password);
    }
}
