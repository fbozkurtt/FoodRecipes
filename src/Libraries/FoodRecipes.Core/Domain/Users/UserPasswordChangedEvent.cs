namespace FoodRecipes.Core.Domain.Users
{
    /// <summary>
    /// Customer password changed event
    /// </summary>
    public class UserPasswordChangedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="password">Password</param>
        public UserPasswordChangedEvent(UserPassword password)
        {
            Password = password;
        }

        /// <summary>
        /// Customer password
        /// </summary>
        public UserPassword Password { get; }
    }
}