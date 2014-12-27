using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NDTV.Controller;
using System.ComponentModel;

namespace NDTV.Entities
{  
    
    /// <summary>
    /// Weather details of the specific details.
    /// </summary>    
    public class WeatherData
    {
        /// <summary>
        /// Gets or sets the city name
        /// </summary>
        public string CityName { get; set; }       

        /// <summary>
        /// Gets or sets the temperature in Celsius
        /// </summary>
        public string TemperatureCelsius { get; set; }        

        /// <summary>
        /// Gets or sets the image URL
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        public string Icon { get; set; }
    }
}
