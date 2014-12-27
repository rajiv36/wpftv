using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class WeatherCitiesRequest : Request
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public WeatherCitiesRequest()
            : base("WeatherDetailsRequest", Utility.GetLink(Constants.LinkNames.WeatherDetailsLink), HttpOperation.Get)
        {

        }

        /// <summary>
        /// Overridden method to build response object
        /// </summary>
        /// <param name="responseString">Response string</param>
        /// <returns>Response object</returns>
        protected override Response BuildResponseObject(string responseString)
        {
            WeatherCitiesResponse response = new WeatherCitiesResponse(responseString);
            return response;
        }
    }
}
