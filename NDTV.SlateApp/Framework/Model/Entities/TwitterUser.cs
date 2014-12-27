
using System.Runtime.Serialization;

namespace NDTV.Entities
{
    [DataContract]
    public class TwitterUser
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the screen_name.
        /// </summary>
        [DataMember(Name = "screen_name")]
        public string ScreenName 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the profile_image_url.
        /// </summary>
        [DataMember(Name = "profile_image_url")]
        public string PhotoLink
        {
            get;
            set;
        }
    }
}
