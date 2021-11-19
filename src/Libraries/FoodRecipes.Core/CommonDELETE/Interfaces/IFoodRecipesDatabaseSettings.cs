namespace FoodRecipes.Core.Common.Interfaces
{
    public interface IFoodRecipesDatabaseSettings
    {
        public string RecipesCollectionName { get; set; }
        public string PicturesCollectionName { get; set; }
        public string UrlRecordsCollectionName { get; set; }
        public string PermissionRecordsCollectionName { get; set; }
        public string PermissionRecordIdentityRoleMappingsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
