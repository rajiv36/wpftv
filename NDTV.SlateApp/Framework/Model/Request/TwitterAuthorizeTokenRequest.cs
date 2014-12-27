
using NDTV.Controller;

namespace NDTV.Entities
{
    public class TwitterAuthorizeTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TwitterAuthorizeTokenRequest(string requestHeader) :
            base("TwitterAuthorizeTokenRequest", GetRequestLink(), HttpOperation.Post, requestHeader, string.Empty, string.Empty, HttpAuthentication.None)
        {
        }

        /// <summary>
        /// Gets the request link.
        /// </summary>
        /// <returns>string</returns>
        private static string GetRequestLink()
        {
            return (ApplicationData.TwitterAccount as TwitterAccount).TokenAccessLink;
        }
    
        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            TwitterAuthorizeTokenResponse response = new TwitterAuthorizeTokenResponse(responseString);
            return response;
        }
    }
}
