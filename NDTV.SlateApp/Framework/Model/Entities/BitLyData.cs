using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Represents the Bit.ly data retrieved from the url shortening call.
    /// </summary>
    [DataContract]
    public class BitLyData
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [DataMember(Name = "status_txt")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the link details.
        /// </summary>
        [DataMember(Name = "data")]
        public LinkDetails LinkDetail { get; set; }
    }

    /// <summary>
    /// Provides funvtionality for link details.
    /// </summary>
    [DataContract]
    public class LinkDetails
    {
        /// <summary>
        /// Gets or sets the original link.
        /// </summary>
        [DataMember(Name = "long_url")]
        public string LongLink { get; set; }

        /// <summary>
        /// Gets or sets the shortened link.
        /// </summary>
        [DataMember(Name = "url")]
        public string ShortenedLink { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "global_hash")]
        public string GlobalHash { get; set; }

        [DataMember(Name = "new_hash")]
        public string NewHash { get; set; }
    }
}
