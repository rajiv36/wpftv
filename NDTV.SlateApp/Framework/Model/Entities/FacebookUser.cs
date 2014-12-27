
using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Facebook user.
    /// </summary>
    [DataContract]
    public class FacebookUser
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
        public dynamic Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the photolink.
        /// </summary>    
        [DataMember(Name = "picture")]
        public string PhotoLink
        {
            get;
            set;
        }
    }
}
