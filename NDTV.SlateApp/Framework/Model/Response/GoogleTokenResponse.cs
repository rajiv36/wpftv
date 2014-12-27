
using NDTV.Utilities;
using System.Collections.Generic;
namespace NDTV.Entities
{
    public class GoogleTokenResponse : Response
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

        public GoogleTokenResponse(string responseValue)
            : base(responseValue)
        {
            this.Parse();
        }

        private void Parse()
        {
            Dictionary<string, string> result = Utility.GetQueryParameters(responseMessage);
            if (result != null && result.Count > 0 && result.ContainsKey(OAuthHelper.OAuthTokenKey) 
                && result.ContainsKey(OAuthHelper.OAuthTokenSecretKey))
            {
                this.authToken = result[OAuthHelper.OAuthTokenKey];
                this.authTokenSecret = result[OAuthHelper.OAuthTokenSecretKey];
            }
        }
    }
}
