using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NDTV.Entities
{
    public class StockDetailRequest :Request
    {
        /// <summary>
        /// Default Constructot 
        /// </summary>
        /// <param name="stockDetailLink">The required stock detail link</param>
        public StockDetailRequest(string stockDetailLink)
            : base("StockDetailRequest", stockDetailLink, HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="stockDetailLink">Appropriate Link</param>
        public StockDetailRequest(string stockDetailLink,int pageNumber)
            : base("StockDetailRequest",BuildLink(stockDetailLink,pageNumber), HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overloaded Constructor 
        /// </summary>
        /// <param name="stockDetailLink">The link</param>
        /// <param name="pageNumber">Associated Page Number</param>
        public StockDetailRequest(string stockDetailLink, int pageNumber,int pageSize)
            : base("StockDetailRequest", BuildLink(stockDetailLink,pageNumber,pageSize), HttpOperation.Get)
        {
        }

        /// <summary>
        /// Overridden methods which builds the appropriate Response Object.
        /// </summary>
        /// <param name="responseString">Response String</param>
        /// <returns>Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            StockDetailResponse response = new StockDetailResponse(responseString);
            return response;
        }

        /// <summary>
        /// Method to build the complete link based on the page number and page size
        /// </summary>
        /// <param name="link">Basic Link</param>
        /// <param name="pageNumber">The Page number</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>Complete Link</returns>
        private static string BuildLink(string link, int pageNumber,int pageSize)
        {
            string completeLink=string.Empty;
            if (false == string.IsNullOrWhiteSpace(link) && pageNumber != 0)
            {
                completeLink = string.Format(CultureInfo.InvariantCulture, link, pageNumber,pageSize);
            }
            return completeLink;
        }

        /// <summary>
        /// Overloaded Method to build the complete link based on the page number
        /// </summary>
        /// <param name="link">Basic Link</param>
        /// <param name="pageNumber">The Page number</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>Complete Link</returns>
        private static string BuildLink(string link, int pageNumber)
        {
            string completeLink = string.Empty;
            if (false == string.IsNullOrWhiteSpace(link) && pageNumber != 0)
            {
                completeLink = string.Format(CultureInfo.InvariantCulture, link, pageNumber);
            }
            return completeLink;
        }


    }
}
