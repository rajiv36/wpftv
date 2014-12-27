using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Class to hold request for Video category List
    /// </summary>
    public class VideoCategoriesRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public VideoCategoriesRequest()
            : base("VideoCategoryList", Utility.GetLink(Constants.LinkNames.VideosFeedLink), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            VideoCategoriesResponse response = new VideoCategoriesResponse(responseString);
            return response;
        }
    }
}
