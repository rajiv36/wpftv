using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Represents the yahoo user.
    /// </summary>
    [DataContract]
    public class YahooUser
    {
        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        [DataMember(Name = "profile")]
        public YahooDetails Details { get; set; }
    }

    /// <summary>
    /// Represents the yahoo user profile data.
    /// </summary>
    [DataContract]
    public class YahooDetails
    {
        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        [DataMember(Name = "guid")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the image of the user.
        /// </summary>
        [DataMember(Name = "image")]
        public Image ImageDetails { get; set; }

        /// <summary>
        /// Gets or sets the nickname of the user.
        /// </summary>
        [DataMember(Name = "nickname")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the gender of the user.
        /// </summary>
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
    }

    /// <summary>
    /// Represents image information of a user.
    /// </summary>
    [DataContract]
    public class Image
    {
        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string PhotoLink
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public string Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        [DataMember(Name = "size")]
        public string Size
        {
            get;
            set;
        }
    }
}
