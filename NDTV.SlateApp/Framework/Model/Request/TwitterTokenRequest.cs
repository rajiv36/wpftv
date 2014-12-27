
using NDTV.Controller;

namespace NDTV.Entities
{
    public class TwitterTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TwitterTokenRequest(string requestHeader) :
            base("TwitterTokenRequest", GetTokenRequestLink(), HttpOperation.Post, requestHeader, string.Empty,
            string.Empty, HttpAuthentication.Basic)
        {            
        }

        /// <summary>
        /// Gets the token request link.
        /// </summary>
        /// <returns></returns>
        private static string GetTokenRequestLink()
        {
            return (ApplicationData.TwitterAccount as TwitterAccount).TokenRequestLink;
        }

         /// <summary>
         /// Overridden method to build response object.
         /// </summary>
         /// <param name="responseString">Response string</param>
         /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            TwitterTokenResponse response = new TwitterTokenResponse(responseString);
            return response;
        }
    }
}
