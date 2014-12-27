using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Represents a Google user.
    /// </summary>
    [DataContract]
    public class GoogleUser
    {
        [DataMember(Name = "entry")]
        public GoogleDetails Details { get; set; }
    }

    [DataContract]
    public class GoogleDetails
    {
        /// <summary>
        /// Gets or sets the profile URL.
        /// </summary>
        [DataMember(Name = "profileUrl")]
        public string ProfileLink { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether viewer or not.
        /// </summary>
        [DataMember(Name = "isViewer")]
        public string IsViewer { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name")]
        public GoogleUserName Name { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        [DataMember(Name = "thumbnailUrl")]
        public string PhotoLink { get; set; }
    }

    /// <summary>
    /// Represents the google user name information.
    /// </summary>
    [DataContract]
    public class GoogleUserName
    {
        /// <summary>
        /// Gets or sets a value indicating format data.
        /// </summary>
        [DataMember(Name = "formatted")]
        public string Short { get; set; }

        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        [DataMember(Name = "familyName")]
        public string Last { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        [DataMember(Name = "givenName")]
        public string Full { get; set; }
    }
}
