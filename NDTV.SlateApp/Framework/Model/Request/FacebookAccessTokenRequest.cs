
using NDTV.Controller;

namespace NDTV.Entities
{
    public class FacebookAccessTokenRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FacebookAccessTokenRequest(string accessTokenLink)
            : base("FacebookAccessTokenRequest", accessTokenLink, HttpOperation.Post)
        {
        }       

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            FacebookAccessTokenResponse response = new FacebookAccessTokenResponse(responseString);
            return response;
        }
    }
}
