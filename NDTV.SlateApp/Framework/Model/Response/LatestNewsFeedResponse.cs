using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Linq;

namespace NDTV.Entities
{
    public class LatestNewsFeedResponse:Response
    {
        #region Main Properties which actively participates in binding

        public LatestNews LatestNewsDetails { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="responseMessage">the response stream thrown back after invocation of the respective API</param>
        public LatestNewsFeedResponse(string responseMessage)
            : base(responseMessage)
        {
            LatestNewsDetails = new LatestNews();
            LatestNewsDetails.BuildGraphObject(base.responseMessage);
        }

        #endregion


    }
}
