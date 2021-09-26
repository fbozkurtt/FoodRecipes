using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Application.Security;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Infrastructure.Persistence;
using Internative.FoodRecipes.Infrastructure.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var host = CreateHostBuilder(args)
                .Build();

            try
            {
                Log.Information("Starting FoodRecipes Web.");
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<IdentityDbContext>();

                        await context.Database.MigrateAsync();

                        Log.Information("Awaiting migrations applied to the identity database (MSSQL SERVER).");
                    }

                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred while migrating the identity database.");
                    }

                    try
                    {
                        var permissionService = services.GetRequiredService<IPermissionService>();
                        var permissionProvider = services.GetRequiredService<IPermissionProvider>();

                        var recipeRepository = services.GetRequiredService<IRepository<Recipe>>();

                        await permissionService.InstallPermissionsAsync(permissionProvider);

                        await SeedDatabase.SeedSampleDataAsync(recipeRepository);


                        Log.Information("Database (MongoDB) seeded with default values.");
                    }

                    catch(Exception ex)
                    {
                        Log.Error(ex, "An error occured while updating the database.");
                    }
                }

                await host.RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(new ConfigurationBuilder()
                       .AddCommandLine(args)
                       .Build());
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog();
                });
    }
}
