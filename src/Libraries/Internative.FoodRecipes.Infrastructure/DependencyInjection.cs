using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Infrastructure.Identity;
using Internative.FoodRecipes.Infrastructure.Persistence;
using Internative.FoodRecipes.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(migrationAssembly)));

            services.Configure<FoodRecipesDatabaseSettings>(
                configuration.GetSection(nameof(FoodRecipesDatabaseSettings)));

            services.AddSingleton<IFoodRecipesDatabaseSettings>(sp =>
                sp.GetRequiredService<FoodRecipesDatabaseSettings>());

            services.AddSingleton<IRepository<Recipe>, Repository<Recipe>>();
            services.AddSingleton<IRepository<PermissionRecordIdentityRoleMapping>, Repository<PermissionRecordIdentityRoleMapping>>();
            services.AddSingleton<IRepository<Picture>, Repository<Picture>>();
            services.AddSingleton<IRepository<PermissionRecord>, Repository<PermissionRecord>>();
            services.AddSingleton<IRepository<UrlRecord>, Repository<UrlRecord>>();

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


            services.AddAuthorization();

            return services;
        }
    }
}
