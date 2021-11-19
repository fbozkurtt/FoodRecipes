using FoodRecipes.Services.ScheduleTasks;

namespace FoodRecipes.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : IScheduleTask
    {
        #region Fields

        private readonly FoodRecipesHttpClient _httpClient;

        #endregion

        #region Ctor

        public KeepAliveTask(FoodRecipesHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            await _httpClient.KeepAliveAsync();
        }

        #endregion
    }
}