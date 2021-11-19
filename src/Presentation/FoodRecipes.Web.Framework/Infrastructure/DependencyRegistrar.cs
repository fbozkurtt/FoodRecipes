using System;
using System.Linq;
using FoodRecipes.Core;
using FoodRecipes.Core.Caching;
using FoodRecipes.Core.Configuration;
using FoodRecipes.Core.Events;
using FoodRecipes.Core.Infrastructure;
using FoodRecipes.Core.Infrastructure.DependencyManagement;
using FoodRecipes.Data;
using FoodRecipes.Services.Authentication;
using FoodRecipes.Services.Common;
using FoodRecipes.Services.Configuration;
using FoodRecipes.Services.Directory;
using FoodRecipes.Services.Events;
using FoodRecipes.Services.Helpers;
using FoodRecipes.Services.Installation;
using FoodRecipes.Services.Localization;
using FoodRecipes.Services.Logging;
using FoodRecipes.Services.Media;
using FoodRecipes.Services.Media.RoxyFileman;
using FoodRecipes.Services.ScheduleTasks;
using FoodRecipes.Services.Security;
using FoodRecipes.Services.Seo;
using FoodRecipes.Services.Users;
using FoodRecipes.Web.Framework.Mvc.Routing;
using FoodRecipes.Web.Framework.UI;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FoodRecipes.Web.Framework.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            //file provider
            services.AddScoped<IFoodRecipesFileProvider, FoodRecipesFileProvider>();

            //web helper
            services.AddScoped<IWebHelper, WebHelper>();

            //user agent helper
            services.AddScoped<IUserAgentHelper, UserAgentHelper>();

            //data layer
            services.AddTransient<IDataProviderManager, DataProviderManager>();
            services.AddTransient(serviceProvider =>
                serviceProvider.GetRequiredService<IDataProviderManager>().DataProvider);

            //repositories
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            //static cache manager
            if (appSettings.Get<DistributedCacheConfig>().Enabled)
            {
                services.AddScoped<ILocker, DistributedCacheManager>();
                services.AddScoped<IStaticCacheManager, DistributedCacheManager>();
            }
            else
            {
                services.AddSingleton<ILocker, MemoryCacheManager>();
                services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            }

            //work context
            services.AddScoped<IWorkContext, WebWorkContext>();

            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IRecentlyViewedRecipesService, RecentlyViewedRecipesService>();
            //services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ISearchTermService, SearchTermService>();
            services.AddScoped<IGenericAttributeService, GenericAttributeService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAclService, AclService>();
            services.AddScoped<IGeoLookupService, GeoLookupService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IDownloadService, DownloadService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
            services.AddScoped<IUrlRecordService, UrlRecordService>();
            services.AddScoped<ILogger, DefaultLogger>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            services.AddScoped<IPageHeadBuilder, PageHeadBuilder>();
            services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
            services.AddSingleton<IRoutePublisher, RoutePublisher>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddScoped<ISettingService, SettingService>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //register all settings
            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
            {
                services.AddScoped(setting,
                    serviceProvider =>
                    {
                        return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
                    });
            }

            services.AddScoped<IPictureService, PictureService>();

            //roxy file manager service
            services.AddTransient<DatabaseRoxyFilemanService>();
            services.AddTransient<FileRoxyFilemanService>();

            services.AddScoped<IRoxyFilemanService>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<IPictureService>().IsStoreInDbAsync().Result
                    ? serviceProvider.GetRequiredService<DatabaseRoxyFilemanService>()
                    : serviceProvider.GetRequiredService<FileRoxyFilemanService>();
            });

            //installation service
            if (!DataSettingsManager.IsDatabaseInstalled())
                services.AddScoped<IInstallationService, InstallationService>();

            //slug route transformer
            if (DataSettingsManager.IsDatabaseInstalled())
                services.AddScoped<SlugRouteTransformer>();

            //schedule tasks
            services.AddSingleton<ITaskScheduler, TaskScheduler>();
            services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();
            if (DataSettingsManager.IsDatabaseInstalled())
                services.AddHostedService<ScheduleTaskHostedService>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
            {
                var isMatch = type.IsGenericType && ((Type) criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                return isMatch;
            }, typeof(IConsumer<>)))
                services.AddScoped(findInterface, consumer);
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}