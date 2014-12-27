
namespace NDTV.Entities
{
    /// <summary>
    /// Request to get the Image Album from categories.
    /// </summary>
    public class ImageAlbumRequest : Request
    {               
        private bool fetchHighResolutionImages;
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageAlbumRequest(string imageAlbumLink)
            : base("ImageAlbumRequest", imageAlbumLink, HttpOperation.Get)
        {
            fetchHighResolutionImages = false;
        }

        /// <summary>
        /// Constructor to fetch high resolution images
        /// </summary>
        /// <param name="imageAlbumLink">Album url</param>
        /// <param name="fetchHighResolutionImages">Get high resolution images</param>
        public ImageAlbumRequest(string imageAlbumLink,bool fetchHighResolutionImages)
            : base("ImageAlbumRequest", imageAlbumLink, HttpOperation.Get)
        {
            this.fetchHighResolutionImages = fetchHighResolutionImages;
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        /// <returns>Response object.</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            ImageAlbumResponse response = new ImageAlbumResponse(responseString,fetchHighResolutionImages);
            return response;
        }
    }
}
