using System;

namespace NDTV.Entities
{
    /// <summary>
    /// This class stores the details of each Image item in the Album.
    /// </summary>
    public class ImageItem
    {
        /// <summary>
        /// Priority of the Image in the Album.
        /// </summary>
        public int ImagePriority
        {
            get;
            set;
        }

        /// <summary>
        /// Content ID of the image.
        /// </summary>
        public int ImageId
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the content image.
        /// </summary>
        public string ImageTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Description in short.
        /// </summary>
        public string ImageShortDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Complete description on the image.
        /// </summary>
        public string ImageCompleteDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Source of the image.Image By:.
        /// </summary>
        public string ImageSource
        {
            get;
            set;
        }

        /// <summary>
        /// Image created date.
        /// </summary>
        public DateTime ImageCreatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Image last updated date.
        /// </summary>
        public DateTime ImageUpdatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the image.
        /// </summary>
        public string ImageThumbnailLink
        {
            get;
            set;
        }

    }
}
