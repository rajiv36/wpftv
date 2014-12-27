
namespace NDTV.Entities
{
    public class ShareData
    {
        /// <summary>
        /// Gets or sets the message data.
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the link data.
        /// </summary>
        public string Link
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image link data.
        /// </summary>
        public string ImageLink
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description data.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the caption data.
        /// </summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public string Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the share type.
        /// </summary>
        public ShareMediaType ShareType
        {
            get;
            set;
        }
    }
}
