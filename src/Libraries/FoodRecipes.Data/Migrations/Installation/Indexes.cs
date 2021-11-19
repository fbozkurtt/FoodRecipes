using FluentMigrator;
using FluentMigrator.SqlServer;
using FoodRecipes.Core.Domain.Seo;
using FoodRecipes.Data.Mapping;
using StackExchange.Redis;

namespace FoodRecipes.Data.Migrations.Installation
{
    [FoodRecipesMigration("2020/03/13 09:36:08:9037677", "FoodRecipes.Data base indexes", MigrationProcessType.Installation)]
    public class Indexes : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_UrlRecord_Slug")
                .OnTable(nameof(UrlRecord))
                .OnColumn(nameof(UrlRecord.Slug))
                .Ascending()
                .WithOptions()
                .NonClustered();

            Create.Index("IX_UrlRecord_Custom_1").OnTable(nameof(UrlRecord))
                .OnColumn(nameof(UrlRecord.EntityId)).Ascending()
                .OnColumn(nameof(UrlRecord.EntityName)).Ascending()
                .OnColumn(nameof(UrlRecord.LanguageId)).Ascending()
                .OnColumn(nameof(UrlRecord.IsActive)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}
