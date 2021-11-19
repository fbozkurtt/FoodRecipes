using System;
using System.Linq;
using System.Threading.Tasks;
using FoodRecipes.Core;
using FoodRecipes.Core.Domain.Users;
using FoodRecipes.Core.Events;
using FoodRecipes.Services.Authentication;
using FoodRecipes.Services.Localization;
using FoodRecipes.Services.Logging;
using FoodRecipes.Services.Security;
using Microsoft.AspNetCore.Mvc;

namespace FoodRecipes.Services.Users
{
    /// <summary>
    /// User registration service
    /// </summary>
    public partial class UserRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        //private readonly INotificationService _notificationService;
        private readonly IWorkContext _workContext;
        //private readonly IWorkflowMessageService _workflowMessageService;

        #endregion

        #region Ctor

        public UserRegistrationService(UserSettings userSettings,
            IAuthenticationService authenticationService,
            IUserActivityService userActivityService,
            IUserService userService,
            IEncryptionService encryptionService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            //INotificationService notificationService,
            IWorkContext workContext//,
            //IWorkflowMessageService workflowMessageService)
            )
        {
            _userSettings = userSettings;
            _authenticationService = authenticationService;
            _userActivityService = userActivityService;
            _userService = userService;
            _encryptionService = encryptionService;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            //_notificationService = notificationService;
            _workContext = workContext;
            //_workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether the entered password matches with a saved one
        /// </summary>
        /// <param name="userPassword">Customer password</param>
        /// <param name="enteredPassword">The entered password</param>
        /// <returns>True if passwords match; otherwise false</returns>
        protected bool PasswordsMatch(UserPassword userPassword, string enteredPassword)
        {
            if (userPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (userPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, userPassword.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
            }

            if (userPassword.Password == null)
                return false;

            return userPassword.Password.Equals(savedPassword);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public virtual async Task<UserLoginResults> ValidateUserAsync(string usernameOrEmail, string password)
        {
            var customer = _userSettings.UsernamesEnabled ?
                await _userService.GetUserByUsernameAsync(usernameOrEmail) :
                await _userService.GetUserByEmailAsync(usernameOrEmail);

            if (customer == null)
                return UserLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return UserLoginResults.Deleted;
            if (!customer.Active)
                return UserLoginResults.NotActive;
            //only registered can login
            if (!await _userService.IsRegisteredAsync(customer))
                return UserLoginResults.NotRegistered;
            //check whether a customer is locked out
            if (customer.CannotLoginUntilDateUtc.HasValue && customer.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return UserLoginResults.LockedOut;

            if (!PasswordsMatch(await _userService.GetCurrentPasswordAsync(customer.Id), password))
            {
                //wrong password
                customer.FailedLoginAttempts++;
                if (_userSettings.FailedPasswordAllowedAttempts > 0 &&
                    customer.FailedLoginAttempts >= _userSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    customer.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_userSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    customer.FailedLoginAttempts = 0;
                }

                await _userService.UpdateUserAsync(customer);

                return UserLoginResults.WrongPassword;
            }

            //update login details
            customer.FailedLoginAttempts = 0;
            customer.CannotLoginUntilDateUtc = null;
            customer.RequireReLogin = false;
            customer.LastLoginDateUtc = DateTime.UtcNow;
            await _userService.UpdateUserAsync(customer);

            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public virtual async Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.User == null)
                throw new ArgumentException("Can't load current user");

            var result = new UserRegistrationResult();
            if (request.User.IsSearchEngineAccount())
            {
                result.AddError("Search engine can't be registered");
                return result;
            }

            if (request.User.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }

            if (await _userService.IsRegisteredAsync(request.User))
            {
                result.AddError("Current user is already registered");
                return result;
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.EmailIsNotProvided"));
                return result;
            }

            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError(await _localizationService.GetResourceAsync("Common.WrongEmail"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.PasswordIsNotProvided"));
                return result;
            }

            if (_userSettings.UsernamesEnabled && string.IsNullOrEmpty(request.Username))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.UsernameIsNotProvided"));
                return result;
            }

            //validate unique user
            if (await _userService.GetUserByEmailAsync(request.Email) != null)
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.EmailAlreadyExists"));
                return result;
            }

            if (_userSettings.UsernamesEnabled && await _userService.GetUserByUsernameAsync(request.Username) != null)
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.Register.Errors.UsernameAlreadyExists"));
                return result;
            }

            //at this point request is valid
            request.User.Username = request.Username;
            request.User.Email = request.Email;

            var userPassword = new UserPassword()
            {
                UserId = request.User.Id,
                PasswordFormat = request.PasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(FoodRecipesUserServicesDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
                    break;
            }

            await _userService.InsertUserPasswordAsync(userPassword);

            request.User.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = await _userService.GetUserRoleBySystemNameAsync(FoodRecipesUserDefaults.RegisteredRoleName);
            if (registeredRole == null)
                throw new FoodRecipesException("'Registered' role could not be loaded");

            await _userService.AddUserRoleMappingAsync(new UserUserRoleMapping() { UserId = request.User.Id, UserRoleId = registeredRole.Id });

            await _userService.UpdateUserAsync(request.User);

            return result;
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public virtual async Task<ChangePasswordResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var result = new ChangePasswordResult();
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.ChangePassword.Errors.EmailIsNotProvided"));
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.ChangePassword.Errors.PasswordIsNotProvided"));
                return result;
            }

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.ChangePassword.Errors.EmailNotFound"));
                return result;
            }

            //request isn't valid
            if (request.ValidateRequest && !PasswordsMatch(await _userService.GetCurrentPasswordAsync(user.Id), request.OldPassword))
            {
                result.AddError(await _localizationService.GetResourceAsync("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
                return result;
            }

            //check for duplicates
            if (_userSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = await _userService.GetUserPasswordsAsync(user.Id, passwordsToReturn: _userSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordsMatch(password, request.NewPassword));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError(await _localizationService.GetResourceAsync("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
                    return result;
                }
            }

            //at this point request is valid
            var userPassword = new UserPassword()
            {
                UserId = user.Id,
                PasswordFormat = request.NewPasswordFormat,
                CreatedOnUtc = DateTime.UtcNow
            };
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    userPassword.Password = request.NewPassword;
                    break;
                case PasswordFormat.Encrypted:
                    userPassword.Password = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(FoodRecipesUserServicesDefaults.PasswordSaltKeySize);
                    userPassword.PasswordSalt = saltKey;
                    userPassword.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey,
                        request.HashedPasswordFormat ?? _userSettings.HashedPasswordFormat);
                    break;
            }

            await _userService.InsertUserPasswordAsync(userPassword);

            //publish event
            await _eventPublisher.PublishAsync(new UserPasswordChangedEvent(userPassword));

            return result;
        }

        /// <summary>
        /// Login passed user
        /// </summary>
        /// <param name="user">User to login</param>
        /// <param name="returnUrl">URL to which the user will return after authentication</param>
        /// <param name="isPersist">Is remember me</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result of an authentication
        /// </returns>
        public virtual async Task<IActionResult> SignInCustomerAsync(User user, string returnUrl, bool isPersist = false)
        {
            if ((await _workContext.GetCurrentUserAsync())?.Id != user.Id)
            {
                await _workContext.SetCurrentUserAsync(user);
            }

            //sign in new customer
            await _authenticationService.SignInAsync(user, isPersist);

            //raise event       
            await _eventPublisher.PublishAsync(new UserLoggedinEvent(user));

            //activity log
            await _userActivityService.InsertActivityAsync(user, "PublicStore.Login",
                await _localizationService.GetResourceAsync("ActivityLog.PublicStore.Login"), user);

            //redirect to the return URL if it's specified
            if (!string.IsNullOrEmpty(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectToRouteResult("Homepage", null);
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">Customer</param>
        /// <param name="newEmail">New email</param>
        /// <param name="requireValidation">Require validation of new email address</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SetEmailAsync(User user, string newEmail, bool requireValidation)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newEmail == null)
                throw new FoodRecipesException("Email cannot be null");

            newEmail = newEmail.Trim();
            var oldEmail = user.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new FoodRecipesException(await _localizationService.GetResourceAsync("Account.EmailUsernameErrors.NewEmailIsNotValid"));

            if (newEmail.Length > 100)
                throw new FoodRecipesException(await _localizationService.GetResourceAsync("Account.EmailUsernameErrors.EmailTooLong"));

            var customer2 = await _userService.GetUserByEmailAsync(newEmail);
            if (customer2 != null && user.Id != customer2.Id)
                throw new FoodRecipesException(await _localizationService.GetResourceAsync("Account.EmailUsernameErrors.EmailAlreadyExists"));

            if (requireValidation)
            {
                //re-validate email
                user.EmailToRevalidate = newEmail;
                await _userService.UpdateUserAsync(user);

                //email re-validation message
                //await _workflowMessageService.SendCustomerEmailRevalidationMessageAsync(user, (await _workContext.GetWorkingLanguageAsync()).Id);
            }
            else
            {
                user.Email = newEmail;
                await _userService.UpdateUserAsync(user);

                if (string.IsNullOrEmpty(oldEmail) || oldEmail.Equals(newEmail, StringComparison.InvariantCultureIgnoreCase))
                    return;
            }
        }

        /// <summary>
        /// Sets a customer username
        /// </summary>
        /// <param name="user">Customer</param>
        /// <param name="newUsername">New Username</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task SetUsernameAsync(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userSettings.UsernamesEnabled)
                throw new FoodRecipesException("Usernames are disabled");

            newUsername = newUsername.Trim();

            if (newUsername.Length > FoodRecipesUserServicesDefaults.UserUsernameLength)
                throw new FoodRecipesException(await _localizationService.GetResourceAsync("Account.EmailUsernameErrors.UsernameTooLong"));

            var user2 = await _userService.GetUserByUsernameAsync(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new FoodRecipesException(await _localizationService.GetResourceAsync("Account.EmailUsernameErrors.UsernameAlreadyExists"));

            user.Username = newUsername;
            await _userService.UpdateUserAsync(user);
        }

        #endregion
    }
}