using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    public class ProfitNewsFlashItemRequest :Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stockDetailLink">Appropriate Link</param>
        public ProfitNewsFlashItemRequest(string newsFlashLink)
            : base("ProfitNewsFlashItemRequest", newsFlashLink, HttpOperation.Get)
        {
        
        }

        /// <summary>
        /// Function to build the response object.
        /// </summary>
        /// <param name="responseString">Response String</param>
        /// <returns>Appropriate Response</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            ProfitNewsFlashItemResponse response = new ProfitNewsFlashItemResponse(responseString);
            return response;
        }

    }
}
