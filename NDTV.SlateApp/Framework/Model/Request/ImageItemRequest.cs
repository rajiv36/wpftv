using System.Globalization;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request to get the Image from the Album.s
    /// </summary>
    public class ImageItemRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageItemRequest(ImageAlbum album)
            : base("ImagesItemRequest",GetAlbumImagesUrl(album) , HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        /// <returns>Response object.</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            ImageItemResponse response = new ImageItemResponse(responseString);
            return response;
        }

        /// <summary>
        /// Gets the link of the images inside an Album.
        /// </summary>
        /// <param name="album">The Album passed.</param>
        /// <returns>Link of Album images.</returns>
        private static string GetAlbumImagesUrl(ImageAlbum album)
        {
            StringBuilder baseUrl = new StringBuilder(Utility.GetLink(Constants.LinkNames.ImageCategoriesAlbumLink));
            baseUrl.Append("?DataFor=AlbumDetail-Paging&PageNumber=1&PageSize=" + album.TotalImagesInAlbum.ToString(CultureInfo.InvariantCulture) + "&AlbumID=" + album.AlbumId.ToString(CultureInfo.InvariantCulture) + "&AlbumType=PG&SortBy=CREATEDDATE&SortOrder=DESC");
            return baseUrl.ToString();
        }
    }
}
