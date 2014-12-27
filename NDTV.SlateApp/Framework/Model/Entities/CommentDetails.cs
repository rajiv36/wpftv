using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NDTV.Entities
{
    /// <summary>
    /// Provides details comment.
    /// </summary>
    [DataContract]
    public class CommentDetails
    {
        /// <summary>
        /// Gets or sets the profile pic of the user who has posted the link.
        /// </summary>
        [DataMember(Name = "userimage")]
        public string ImageLink { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who posted the comment.
        /// </summary>
        [DataMember(Name = "name")]
        public string PostedBy { get; set; }

        /// <summary>
        /// Gets or sets the posted date of the comment.
        /// </summary>
        [DataMember(Name = "created")]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [DataMember(Name = "comment")]
        public string DetailsComment { get; set; }
    }

    /// <summary>
    /// Provides the paging details for the comments.
    /// </summary>
    [DataContract]
    public class PageDetails
    {
        [DataMember(Name = "totalcount", IsRequired = false)]
        public int TotalCount { get; set; }

        [DataMember(Name = "results_per_page", IsRequired = false)]
        public int ResultsPerPage { get; set; }

        [DataMember(Name = "page", IsRequired = false)]
        public int PageInView { get; set; }
    }

    /// <summary>
    /// Provides both the comment list and the paging details for the same. 
    /// </summary>
    [DataContract]
    public class Comments
    {
        /// <summary>
        /// Gets or sets the comment details.
        /// </summary>
        [DataMember(Name = "items")]
        public List<CommentDetails> CommentList { get; set; }

        /// <summary>
        /// Gets or sets the paging details.
        /// </summary>
        [DataMember(Name = "pager", IsRequired = false)]
        public PageDetails PagingDetails { get; set; }
    }
}
