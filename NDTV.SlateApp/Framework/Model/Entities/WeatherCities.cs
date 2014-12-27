using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// Class for list of Indian and foreign cities for weather information.
    /// </summary>
    public class WeatherCities
    {
        /// <summary>
        /// List of Indian cities.
        /// </summary>
        public List<string> IndianCities
        {
            get;
            set;
        }
        
        /// <summary>
        /// List of foreign cities.
        /// </summary>
        public List<string> ForeignCities
        {
            get;
            set;
        }
    }
}
