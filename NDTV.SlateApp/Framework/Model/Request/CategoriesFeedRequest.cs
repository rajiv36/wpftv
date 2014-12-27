using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Request to get the categories Feed
    /// </summary>
    public class CategoriesFeedRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CategoriesFeedRequest()
            : base("CategoriesFeedRequest", Utility.GetLink(Constants.LinkNames.CategoriesFeedLink), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            CategoriesFeedResponse response = new CategoriesFeedResponse(responseString);
            return response;
        }
    }
}
