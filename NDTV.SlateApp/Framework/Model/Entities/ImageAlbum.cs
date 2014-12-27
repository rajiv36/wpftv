using System;

namespace NDTV.Entities
{
    /// <summary>
    /// This class stores the Album listed in categories.
    /// </summary>
    public class ImageAlbum : Item
    {
        /// <summary>
        /// ID of the Album.
        /// </summary>
        public int AlbumId
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the Album.
        /// </summary>
        public string AlbumTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the album.
        /// </summary>
        public string AlbumDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Total number of related images stored in the Album.
        /// </summary>
        public int TotalImagesInAlbum
        {
            get;
            set;
        }

        /// <summary>
        /// Date and time the Album is published.
        /// </summary>
        public DateTime PublishedDataTimeOfAlbum
        {
            get;
            set;
        }

        /// <summary>
        /// Folder Path
        /// </summary>
        public string FolderPath
        {
            get;
            set;
        }

        /// <summary>
        /// Album cover image.
        /// </summary>
        public string AlbumCoverImageName
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the ThumbNail.
        /// </summary>
        public string ThumbnailLink
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the Album from NDTV website.
        /// </summary>
        public string AlbumLink
        {
            get;
            set;
        }
    }
}
