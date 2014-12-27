
namespace NDTV.Entities
{
    public class LinkedInTokenRequest : Request
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LinkedInTokenRequest(string requestLink, string requestHeader) :
            base("LinkedInTokenRequest", requestLink, HttpOperation.Post, requestHeader, string.Empty, string.Empty, HttpAuthentication.Basic)
        {
        }
       
        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            LinkedInTokenResponse response = new LinkedInTokenResponse(responseString);
            return response;
        }
    }
}
