
namespace NDTV.Entities
{
    public class YahooAuthorizeTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="link">Request link.</param>
        public YahooAuthorizeTokenRequest(string link)
            : base("YahooAuthorizeTokenRequest", link, HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            YahooAuthorizeTokenResponse response = new YahooAuthorizeTokenResponse(responseString);
            return response;
        }
    }
}
