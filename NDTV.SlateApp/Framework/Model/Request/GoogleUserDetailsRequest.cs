
namespace NDTV.Entities
{
    public class GoogleUserDetailsRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="header">string - header</param>
        public GoogleUserDetailsRequest(string header)
            : base("GoogleUserDetailsRequest", Constants.GoogleConstants.GetCredentialsUrl, HttpOperation.Get, header, string.Empty, 
            string.Empty, HttpAuthentication.None)
        {
        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            GoogleUserDetailsResponse response = new GoogleUserDetailsResponse(responseString);
            return response;
        }
    }
}
