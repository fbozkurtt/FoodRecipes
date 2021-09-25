using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Domain.Common;
using Internative.FoodRecipes.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Persistence
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        //private readonly ICurrentUserService _currentUserService;

        #region Ctor

        public IdentityDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService) : base(options)
        {
            //_currentUserService = currentUserService;
        }

        #endregion

        #region Utilities

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        #endregion
    }
}
