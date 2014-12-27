
namespace NDTV.Entities
{
    public class GoogleAuthorizeTokenRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="header">string - header</param>
        public GoogleAuthorizeTokenRequest(string header)
            : base("GoogleAuthorizeTokenRequest", Constants.GoogleConstants.AccessTokenUrl, HttpOperation.Post, header, string.Empty, string.Empty, HttpAuthentication.None)
        {
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            GoogleAuthorizeTokenResponse response = new GoogleAuthorizeTokenResponse(responseString);
            return response;
        }
    }
}
