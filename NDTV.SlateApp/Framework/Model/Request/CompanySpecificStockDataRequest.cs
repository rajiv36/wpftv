using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NDTV.Entities
{
    public class CompanySpecificStockDataRequest : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Company Id</param>
        public CompanySpecificStockDataRequest(int id)
            : base("CompanySpecificStockDataRequest", GetCompleteUrl(id), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Function that builds the response object.
        /// </summary>
        /// <param name="responseString">Response String</param>
        /// <returns>Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            CompanySpecificStockDataResponse response = new CompanySpecificStockDataResponse(responseString);
            return response;
        }

        /// <summary>
        /// Returns the appropriate Url based on the Company Id.
        /// </summary>
        /// <param name="id">Company Id</param>
        /// <returns>The Url associated with the company.</returns>
        private static string GetCompleteUrl(int id)
        {
            return (string.Format(CultureInfo.InvariantCulture,Utilities.Utility.GetLink(Constants.LinkNames.CompanySpecificDataLink), id.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
