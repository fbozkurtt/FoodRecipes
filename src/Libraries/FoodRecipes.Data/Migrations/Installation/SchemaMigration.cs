using FluentMigrator;
using FoodRecipes.Core.Domain.Configuration;
using FoodRecipes.Core.Domain.Directory;
using FoodRecipes.Core.Domain.Localization;
using FoodRecipes.Core.Domain.Logging;
using FoodRecipes.Core.Domain.Media;
using FoodRecipes.Core.Domain.Recipes;
using FoodRecipes.Core.Domain.Security;
using FoodRecipes.Core.Domain.Seo;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Data.Extensions;
namespace FoodRecipes.Data.Migrations.Installation
{
    [FoodRecipesMigration("2021/10/25 12:00:00:0000000", "FoodRecipes.Data base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {
            Create.TableFor<Country>();
            Create.TableFor<User>();
            Create.TableFor<UserPassword>();
            Create.TableFor<UserRole>();
            Create.TableFor<UserUserRoleMapping>();
            Create.TableFor<Language>();
            Create.TableFor<LocaleStringResource>();
            Create.TableFor<LocalizedProperty>();
            Create.TableFor<Category>();
            Create.TableFor<Download>();
            Create.TableFor<Picture>();
            Create.TableFor<PictureBinary>();
            Create.TableFor<RecipePicture>();
            Create.TableFor<Setting>();
            Create.TableFor<ActivityLogType>();
            Create.TableFor<ActivityLog>();
            Create.TableFor<Log>();
            Create.TableFor<PermissionRecord>();
            Create.TableFor<UrlRecord>();
        }
    }
}
