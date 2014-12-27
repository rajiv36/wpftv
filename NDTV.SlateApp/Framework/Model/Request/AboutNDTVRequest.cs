using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class AboutNDTVRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AboutNDTVRequest()
            : base("AboutNDTVRequest", Utility.GetLink(Constants.LinkNames.AboutNDTVLink), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object.
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            AboutNDTVResponse response = new AboutNDTVResponse(responseString);
            return response;
        }
    }
}
