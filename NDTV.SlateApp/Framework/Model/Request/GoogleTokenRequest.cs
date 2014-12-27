using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    public class GoogleTokenRequest : Request
    {
        public GoogleTokenRequest(string header, string postData) :
            base("", "https://www.google.com/accounts/OAuthGetRequestToken?", HttpOperation.Post, header, string.Empty, string.Empty, HttpAuthentication.None)
        {
            this.rawRequest = postData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        protected override Response BuildResponseObject(string responseString)
        {
            GoogleTokenResponse response = new GoogleTokenResponse(responseString);
            return response;
        }
    }
}
