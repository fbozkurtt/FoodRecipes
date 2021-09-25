using Internative.FoodRecipes.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => Convert.ToInt32((_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)));

        public string Username => (_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));
    }
}
