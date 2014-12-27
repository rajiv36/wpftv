
using System.Collections.Generic;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class TwitterAuthorizeTokenResponse : Response
    {
        /// <summary>
        /// Gets the authorization token.
        /// </summary>
        private string authToken;
        public string AuthToken
        {
            get
            {
                return this.authToken;
            }
        }

        /// <summary>
        /// Gets the authorization token secret.
        /// </summary>
        private string authTokenSecret;
        public string AuthTokenSecret
        {
            get
            {
                return this.authTokenSecret;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseValue">Response string</param>
        public TwitterAuthorizeTokenResponse(string responseValue) :
            base(responseValue)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response to obtain data.
        /// </summary>
        private void Parse()
        {
            //// Get the query parameters out of the URL response.
            Dictionary<string, string> result = Utility.GetQueryParameters(responseMessage);

            if (result != null && result.Count > 0 && result.ContainsKey(OAuthHelper.OAuthTokenKey) && result.ContainsKey(OAuthHelper.OAuthTokenSecretKey))
            {
                this.authToken = result[OAuthHelper.OAuthTokenKey];
                this.authTokenSecret = result[OAuthHelper.OAuthTokenSecretKey];
            }
        }
    }
}
