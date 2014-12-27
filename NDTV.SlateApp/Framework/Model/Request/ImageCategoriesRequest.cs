using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request to get the categories available. 
    /// </summary>
    public class ImageCategoriesRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageCategoriesRequest()
            : base("ImageCategoriesRequest", Utility.GetLink(Constants.LinkNames.ImageCategoriesLink), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        /// <returns>Response object.</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            ImageCategoriesResponse response = new ImageCategoriesResponse(responseString);
            return response;
        }
    }
}
