using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    public class LatestNewsFeedRequest:Request
    {
        #region Constructor

        /// <summary>
        /// Constructor which loads most of the required properties in the base class which would be helpful while 
        /// </summary>
        public LatestNewsFeedRequest():base("LatestNewsFeed",Utilities.Utility.GetLink(Constants.LinkNames.LatestNewsLink),HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overloaded Constructor called based on the Category selection.
        /// This Constructor will take care of the entire process.
        /// </summary>
        /// <param name="categoryItem">The Category that is selected</param>
        public LatestNewsFeedRequest(CategoryItem categoryItem)
            : base("LatestNewsFeed", categoryItem.Link, HttpOperation.Get)
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Overriding the Response Object Creating method of the Response baseclass..
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        protected override Response BuildResponseObject(string responseString)
        {
            return new LatestNewsFeedResponse(responseString);
        }

        #endregion
    }
}
