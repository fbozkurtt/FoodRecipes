using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using FoodRecipes.Core;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Core.Domain.Localization;
using FoodRecipes.Core.Http;
using FoodRecipes.Core.Security;
using FoodRecipes.Services.Authentication;
using FoodRecipes.Services.Common;
using FoodRecipes.Services.Users;
using FoodRecipes.Services.Helpers;
using FoodRecipes.Services.Localization;
using FoodRecipes.Services.ScheduleTasks;
using FoodRecipes.Web.Framework.Globalization;

namespace FoodRecipes.Web.Framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly CookieSettings _cookieSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILanguageService _languageService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IWebHelper _webHelper;
        private readonly LocalizationSettings _localizationSettings;

        private User _cachedUser;
        private User _originalUserIfImpersonated;
        private Language _cachedLanguage;

        #endregion

        #region Ctor

        public WebWorkContext(CookieSettings cookieSettings,
            IAuthenticationService authenticationService,
            IUserService customerService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            ILanguageService languageService,
            IUserAgentHelper userAgentHelper,
            IWebHelper webHelper,
            LocalizationSettings localizationSettings)
        {
            _cookieSettings = cookieSettings;
            _authenticationService = authenticationService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _languageService = languageService;
            _userAgentHelper = userAgentHelper;
            _webHelper = webHelper;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get nop customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetUserCookie()
        {
            var cookieName = $"{FoodRecipesCookieDefaults.Prefix}{FoodRecipesCookieDefaults.UserCookie}";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }

        /// <summary>
        /// Set nop customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetUserCookie(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response?.HasStarted ?? true)
                return;

            //delete current cookie value
            var cookieName = $"{FoodRecipesCookieDefaults.Prefix}{FoodRecipesCookieDefaults.UserCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = _cookieSettings.UserCookieExpires;
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                Secure = _webHelper.IsCurrentConnectionSecured()
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, customerGuid.ToString(), options);
        }

        /// <summary>
        /// Set language culture cookie
        /// </summary>
        /// <param name="language">Language</param>
        protected virtual void SetLanguageCookie(Language language)
        {
            if (_httpContextAccessor.HttpContext?.Response?.HasStarted ?? true)
                return;

            //delete current cookie value
            var cookieName = $"{FoodRecipesCookieDefaults.Prefix}{FoodRecipesCookieDefaults.CultureCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            if (string.IsNullOrEmpty(language?.LanguageCulture))
                return;

            //set new cookie value
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language.LanguageCulture));
            var options = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, value, options);
        }

        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the found language
        /// </returns>
        protected virtual async Task<Language> GetLanguageFromRequestAsync()
        {
            var requestCultureFeature = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
            if (requestCultureFeature is null)
                return null;

            //whether we should detect the current language by customer settings
            if (requestCultureFeature.Provider is not FoodRecipesSeoUrlCultureProvider && !_localizationSettings.AutomaticallyDetectLanguage)
                return null;

            //get request culture
            if (requestCultureFeature.RequestCulture is null)
                return null;

            //try to get language by culture name
            var requestLanguage = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCultureFeature.RequestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published)
                return null;

            return requestLanguage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current customer
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<User> GetCurrentUserAsync()
        {
            //whether there is a cached value
            if (_cachedUser != null)
                return _cachedUser;

            await SetCurrentUserAsync();

            return _cachedUser;
        }

        /// <summary>
        /// Sets the current customer
        /// </summary>
        /// <param name="customer">Current customer</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SetCurrentUserAsync(User customer = null)
        {
            if (customer == null)
            {
                //check whether request is made by a background (schedule) task
                if (_httpContextAccessor.HttpContext?.Request
                    ?.Path.Equals(new PathString($"/{FoodRecipesTaskDefaults.ScheduleTaskPath}"), StringComparison.InvariantCultureIgnoreCase)
                    ?? true)
                {
                    //in this case return built-in customer record for background task
                    customer = await _customerService.GetOrCreateBackgroundTaskUserAsync();
                }

                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    //check whether request is made by a search engine, in this case return built-in customer record for search engines
                    if (_userAgentHelper.IsSearchEngine())
                        customer = await _customerService.GetOrCreateSearchEngineUserAsync();
                }

                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    //try to get registered user
                    customer = await _authenticationService.GetAuthenticatedUserAsync();
                }

                if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
                {
                    //get impersonate user if required
                    var impersonatedUserId = await _genericAttributeService
                        .GetAttributeAsync<int?>(customer, FoodRecipesUserDefaults.ImpersonatedUserIdAttribute);
                    if (impersonatedUserId.HasValue && impersonatedUserId.Value > 0)
                    {
                        var impersonatedUser = await _customerService.GetUserByIdAsync(impersonatedUserId.Value);
                        if (impersonatedUser != null && !impersonatedUser.Deleted &&
                            impersonatedUser.Active &&
                            !impersonatedUser.RequireReLogin)
                        {
                            //set impersonated customer
                            _originalUserIfImpersonated = customer;
                            customer = impersonatedUser;
                        }
                    }
                }

                if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
                {
                    //get guest customer
                    var customerCookie = GetUserCookie();
                    if (Guid.TryParse(customerCookie, out var customerGuid))
                    {
                        //get customer from cookie (should not be registered)
                        var customerByCookie = await _customerService.GetUserByGuidAsync(customerGuid);
                        if (customerByCookie != null && !await _customerService.IsRegisteredAsync(customerByCookie))
                            customer = customerByCookie;
                    }
                }
            }

            if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
            {
                //set customer cookie
                SetUserCookie(customer.UserGuid);

                //cache the found customer
                _cachedUser = customer;
            }
        }

        /// <summary>
        /// Gets the original customer (in case the current one is impersonated)
        /// </summary>
        public virtual User OriginalUserIfImpersonated => _originalUserIfImpersonated;

        /// <summary>
        /// Sets current user working language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SetWorkingLanguageAsync(Language language)
        {
            //save passed language identifier
            var customer = await GetCurrentUserAsync();
            await _genericAttributeService.SaveAttributeAsync(customer, FoodRecipesUserDefaults.LanguageIdAttribute, language?.Id ?? 0);

            //set cookie
            SetLanguageCookie(language);

            //then reset the cached value
            _cachedLanguage = null;
        }

        /// <summary>
        /// Gets current user working language
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<Language> GetWorkingLanguageAsync()
        {
            //whether there is a cached value
            if (_cachedLanguage != null)
                return _cachedLanguage;

            var customer = await GetCurrentUserAsync();

            //whether we should detect the language from the request
            var detectedLanguage = await GetLanguageFromRequestAsync();

            //get current saved language identifier
            var currentLanguageId = await _genericAttributeService
                .GetAttributeAsync<int>(customer, FoodRecipesUserDefaults.LanguageIdAttribute);

            //if the language is detected we need to save it
            if (detectedLanguage != null)
            {
                //save the detected language identifier if it differs from the current one
                if (detectedLanguage.Id != currentLanguageId)
                    await SetWorkingLanguageAsync(detectedLanguage);
            }
            else
            {
                var allStoreLanguages = await _languageService.GetAllLanguagesAsync();

                //check customer language availability
                detectedLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == currentLanguageId);

                //it not found, then try to get the default language for the current store (if specified)
                //detectedLanguage ??= allStoreLanguages.FirstOrDefault(language => language.Id == store.DefaultLanguageId);

                //if the default language for the current store not found, then try to get the first one
                detectedLanguage ??= allStoreLanguages.FirstOrDefault();

                //if there are no languages for the current store try to get the first one regardless of the store
                detectedLanguage ??= (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();

                SetLanguageCookie(detectedLanguage);
            }

            //cache the found language
            _cachedLanguage = detectedLanguage;

            return _cachedLanguage;
        }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}