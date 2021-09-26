using Internative.FoodRecipes.Application;
using Internative.FoodRecipes.Infrastructure;
using Internative.FoodRecipes.Web.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services, IConfiguration configuration)
        {           
            services.AddApplication(Assembly.GetExecutingAssembly());
            services.AddHttpContextAccessor();
            services.AddInfrastructure(configuration);
            services.AddSingleton<IRecipeModelFactory, RecipeModelFactory>();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.UseMemberCasing());
            return services;
        }
    }
}
