using NDTV.Controller;

namespace NDTV.Entities
{
    public class LinkedInAuthorizeTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LinkedInAuthorizeTokenRequest(string requestLink, string requestHeader) :
            base("LinkedInAuthorizeTokenRequest", requestLink, HttpOperation.Post, requestHeader, string.Empty, string.Empty, HttpAuthentication.None)
        {
        }      

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            LinkedInAuthorizeTokenResponse response = new LinkedInAuthorizeTokenResponse(responseString);
            return response;
        }
    }
}