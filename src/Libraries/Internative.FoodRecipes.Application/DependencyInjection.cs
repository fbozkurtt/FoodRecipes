using AutoMapper;
using Internative.FoodRecipes.Application.Common.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly)
        {
            var mapperConfigurations = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.Name == nameof(IOrderedMapperProfile)))
                .ToList();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
