
namespace NDTV.Entities
{
    public class UserData
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the profile link.
        /// </summary>
        public string ProfileLink
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the login mode.
        /// </summary>
        public LoginMode LoginMode
        {
            get;
            set;
        }
    }
}
