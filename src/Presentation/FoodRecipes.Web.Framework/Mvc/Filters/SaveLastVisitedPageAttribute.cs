using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FoodRecipes.Core;
using FoodRecipes.Core.Domain.Common;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Data;
using FoodRecipes.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodRecipes.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents filter attribute that saves last visited page by customer
    /// </summary>
    public sealed class SaveLastVisitedPageAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        public SaveLastVisitedPageAttribute() : base(typeof(SaveLastVisitedPageFilter))
        {
        }

        #endregion

        #region Nested filter

        /// <summary>
        /// Represents a filter that saves last visited page by customer
        /// </summary>
        private class SaveLastVisitedPageFilter : IAsyncActionFilter
        {
            #region Fields

            private readonly UserSettings _userSettings;
            private readonly IGenericAttributeService _genericAttributeService;
            private readonly IRepository<GenericAttribute> _genericAttributeRepository;
            private readonly IWebHelper _webHelper;
            private readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public SaveLastVisitedPageFilter(UserSettings userSettings,
                IGenericAttributeService genericAttributeService,
                IRepository<GenericAttribute> genericAttributeRepository,
                IWebHelper webHelper,
                IWorkContext workContext)
            {
                _userSettings = userSettings;
                _genericAttributeService = genericAttributeService;
                _genericAttributeRepository = genericAttributeRepository;
                _webHelper = webHelper;
                _workContext = workContext;
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Called asynchronously before the action, after model binding is complete.
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <returns>A task that represents the asynchronous operation</returns>
            private async Task SaveLastVisitedPageAsync(ActionExecutingContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                if (context.HttpContext.Request == null)
                    return;

                //only in GET requests
                if (!context.HttpContext.Request.Method.Equals(WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase))
                    return;

                if (!await DataSettingsManager.IsDatabaseInstalledAsync())
                    return;

                //check whether we store last visited page URL
                if (!_userSettings.StoreLastVisitedPage)
                    return;

                //get current page
                var pageUrl = _webHelper.GetThisPageUrl(true);

                if (string.IsNullOrEmpty(pageUrl))
                    return;

                //get previous last page
                var customer = await _workContext.GetCurrentUserAsync();
                var previousPageAttribute = (await _genericAttributeService
                    .GetAttributesForEntityAsync(customer.Id, nameof(User)))
                    .FirstOrDefault(attribute => attribute.Key
                        .Equals(FoodRecipesUserDefaults.LastVisitedPageAttribute, StringComparison.InvariantCultureIgnoreCase));

                //save new one if don't match
                if (previousPageAttribute == null)
                {
                    //insert without event notification
                    await _genericAttributeRepository.InsertAsync(new GenericAttribute
                    {
                        EntityId = customer.Id,
                        Key = FoodRecipesUserDefaults.LastVisitedPageAttribute,
                        KeyGroup = nameof(User),
                        Value = pageUrl,
                        CreatedOrUpdatedDateUTC = DateTime.UtcNow
                    }, false);
                }
                else if (!pageUrl.Equals(previousPageAttribute.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    //update without event notification
                    previousPageAttribute.Value = pageUrl;
                    previousPageAttribute.CreatedOrUpdatedDateUTC = DateTime.UtcNow;

                    await _genericAttributeRepository.UpdateAsync(previousPageAttribute, false);
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called asynchronously before the action, after model binding is complete.
            /// </summary>
            /// <param name="context">A context for action filters</param>
            /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
            /// <returns>A task that represents the asynchronous operation</returns>
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                await SaveLastVisitedPageAsync(context);
                if (context.Result == null)
                    await next();
            }

            #endregion
        }

        #endregion
    }
}