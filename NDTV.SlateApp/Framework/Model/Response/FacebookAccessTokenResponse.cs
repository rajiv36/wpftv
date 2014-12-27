
using System.Collections.Generic;
using NDTV.Utilities;
namespace NDTV.Entities
{
    public class FacebookAccessTokenResponse : Response
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        private string accessToken;
        public string AccessToken
        {
            get
            {
                return this.accessToken;
            }
        }

        /// <summary>
        /// Gets the expires value.
        /// </summary>
        private string expires;
        public string Expires
        {
            get
            {
                return this.expires;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseMessage">string - Response message</param>
        public FacebookAccessTokenResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// Parses the response message.
        /// </summary>
        private void Parse()
        {
            Dictionary<string, string> result = Utility.GetQueryParameters(responseMessage);
            if (result != null && result.Count > 0)
            {
                if (result.ContainsKey("access_token"))
                {
                    this.accessToken = result["access_token"];

                    if (result.ContainsKey("expires"))
                    {
                        this.expires = result["expires"];
                    }
                }
            }
        }
    }
}
