using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Controller;
using System.Collections.ObjectModel;

namespace NDTV.Entities
{
    /// <summary>
    /// Request for related videos
    /// </summary>
    public class RelatedVideosRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RelatedVideosRequest(string videoLink)
            : base("RelatedVideoRequest", videoLink, HttpOperation.Get)
        {
        }


        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            RelatedVideosResponse response = new RelatedVideosResponse(responseString);
            return response;
        }
    }
}
