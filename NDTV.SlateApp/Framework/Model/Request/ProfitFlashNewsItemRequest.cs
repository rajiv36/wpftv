using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    public class ProfitFlashNewsItemRequest :Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stockDetailLink">Appropriate Link</param>
        public ProfitFlashNewsItemRequest(string newsflashLink)
            : base("ProfitFlashNewsItemRequest", newsflashLink, HttpOperation.Get)
        {
        
        }

        /// <summary>
        /// Function to build the response object.
        /// </summary>
        /// <param name="responseString">Response String</param>
        /// <returns>Appropriate Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            ProfitFlashNewsItemResponse response = new ProfitFlashNewsItemResponse(responseString);
            return response;
        }

    }
}
