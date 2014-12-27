
namespace NDTV.Entities
{
    public class YahooUserDataRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="link">Request link.</param>
        public YahooUserDataRequest(string link)
            :base("YahooUserDataRequest",link, HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            YahooUserDataResponse response = new YahooUserDataResponse(responseString);
            return response;
        }
    }
}
