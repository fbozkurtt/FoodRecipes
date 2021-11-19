using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation.Validators;
using FoodRecipes.Core.Domain.Users;

namespace FoodRecipes.Web.Framework.Validators
{
    /// <summary>
    /// Phohe number validator
    /// </summary>
    public class PhoneNumberPropertyValidator : PropertyValidator
    {
        private readonly UserSettings _userSettings;

        /// <summary>
        /// Ctor
        /// </summary>
        public PhoneNumberPropertyValidator(UserSettings userSettings)
        {
            _userSettings = userSettings;
        }

        protected override string GetDefaultMessageTemplate() => "Phone number is not valid";

        /// <summary>
        /// Is valid?
        /// </summary>
        /// <param name="context">Validation context</param>
        /// <returns>Result</returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return IsValid(context.PropertyValue as string, _userSettings);
        }

        /// <summary>
        /// Is valid?
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="userSettings">Customer settings</param>
        /// <returns>Result</returns>
        public static bool IsValid(string phoneNumber, UserSettings userSettings)
        {
            if (!userSettings.PhoneNumberValidationEnabled || string.IsNullOrEmpty(userSettings.PhoneNumberValidationRule))
                return true;

            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            return userSettings.PhoneNumberValidationUseRegex
                ? Regex.IsMatch(phoneNumber, userSettings.PhoneNumberValidationRule, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase)
                : phoneNumber.All(l => userSettings.PhoneNumberValidationRule.Contains(l));
        }
    }
}
