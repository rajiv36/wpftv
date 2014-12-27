
using System.Collections.Generic;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class YahooAuthorizeTokenResponse : Response
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
        /// Gets the authorization guid.
        /// </summary>        
        public string AuthYahooGuid
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseValue">Response string.</param>
        public YahooAuthorizeTokenResponse(string responseValue)
            : base(responseValue)
        {
            this.Parse();
        }

        /// <summary>
        /// Parses the response string.
        /// </summary>
        private void Parse()
        {
            Dictionary<string, string> result = Utility.GetQueryParameters(responseMessage);
            if (null != result && result.Count > 0 && result.ContainsKey(OAuthHelper.OAuthTokenKey) && result.ContainsKey(OAuthHelper.OAuthTokenSecretKey)
                && result.ContainsKey("xoauth_yahoo_guid"))
            {
                this.AuthToken = result[OAuthHelper.OAuthTokenKey];
                this.AuthTokenSecret = result[OAuthHelper.OAuthTokenSecretKey];
                this.AuthYahooGuid = result["xoauth_yahoo_guid"];
            }
        }
    }
}
