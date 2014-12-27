
using System.Collections.Generic;
using NDTV.Utilities;
namespace NDTV.Entities
{
    public class YahooTokenResponse : Response
    {
        /// <summary>
        /// Gets the authorization token.
        /// </summary>        
        public string AuthToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the authorization token secret.
        /// </summary>        
        public string AuthTokenSecret
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseString">Response string.</param>
        public YahooTokenResponse(string responseString)
            : base(responseString)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response string.
        /// </summary>
        private void Parse()
        {
            Dictionary<string, string> result = Utility.GetQueryParameters(responseMessage);
            if (result != null && result.Count > 0 && result.ContainsKey(OAuthHelper.OAuthTokenKey)
                && result.ContainsKey(OAuthHelper.OAuthTokenSecretKey))
            {
                this.AuthToken = result[OAuthHelper.OAuthTokenKey];
                this.AuthTokenSecret = result[OAuthHelper.OAuthTokenSecretKey];
            }
        }
    }
}
