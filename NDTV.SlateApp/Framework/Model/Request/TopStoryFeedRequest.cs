using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class TopStoryFeedRequest:Request
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TopStoryFeedRequest()
            :base("TopStoryFeed",Utility.GetLink(Constants.LinkNames.TopStoryFeedLink),HttpOperation.Get)
        {

        }

        /// <summary>
        /// Constructor used for filtering News Items based on Category
        /// </summary>
        /// <param name="categoryItem"></param>
        public TopStoryFeedRequest(CategoryItem categoryItem)
            :base("TopStoryFeed",categoryItem.Link,HttpOperation.Get)
        {

        }

        /// <summary>
        /// Constructor used for filtering News Items based on Category Read,Commented which directly takes an URI..
        /// </summary>
        /// <param name="categoryItem"></param>
        public TopStoryFeedRequest(string link)
            : base("TopStoryFeed", link, HttpOperation.Get)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the response object from the returned back stream..
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        protected override Response BuildResponseObject(string responseString)
        {
            return new TopStoryFeedResponse(responseString);
        }

        #endregion
    }
}
