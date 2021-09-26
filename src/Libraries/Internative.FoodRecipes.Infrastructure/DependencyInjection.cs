using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Application.Security;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Infrastructure.Identity;
using Internative.FoodRecipes.Infrastructure.Persistence;
using Internative.FoodRecipes.Infrastructure.Security;
using Internative.FoodRecipes.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Internative.FoodRecipes.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string migrationAssembly = typeof(IdentityDbContext).Assembly.FullName;

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IRecipeService, RecipeService>();

            services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(migrationAssembly)));

            services.Configure<FoodRecipesDatabaseSettings>(
                configuration.GetSection(nameof(FoodRecipesDatabaseSettings)));

            services.AddSingleton<IFoodRecipesDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<FoodRecipesDatabaseSettings>>().Value);

            services.AddScoped<IRepository<Recipe>, Repository<Recipe>>();
            services.AddScoped<IRepository<PermissionRecordIdentityRoleMapping>, Repository<PermissionRecordIdentityRoleMapping>>();
            services.AddScoped<IRepository<Picture>, Repository<Picture>>();
            services.AddScoped<IRepository<PermissionRecord>, Repository<PermissionRecord>>();
            services.AddScoped<IRepository<UrlRecord>, Repository<UrlRecord>>();

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.Password = IdentitySettings.GetPasswordOptions();
            })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),
                    };
                });

            services.AddScoped<IPermissionProvider, PermissionProvider>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddAuthorization();

            return services;
        }
    }
}
