
namespace NDTV.Entities
{
    /// <summary>
    /// This class stores the type of category it contains like SPORTS,ENTERTAINTMENT etc.
    /// </summary>
    public class ImageCategoryItem
    {
        /// <summary>
        /// Title of category.
        /// </summary>
        public string ImageAlbumTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Link of the category.
        /// </summary>
        public string ImageAlbumLink
        {
            get;
            set;
        }

        /// <summary>
        /// Priority of the category.
        /// </summary>
        public int Priority 
        {
            get;
            set;
        }

        /// <summary>
        /// Property added as a helper for checking/Un checking the options buttons(Categories) on the Carousel View.
        /// </summary>
        public bool IsCheckedProperty
        {
            get;
            set;
        }
    }
}
