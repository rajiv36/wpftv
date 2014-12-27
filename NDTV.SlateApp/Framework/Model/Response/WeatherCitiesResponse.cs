using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JavaScriptSerializer.Serialization;
using NDTV.Controller;

namespace NDTV.Entities
{
    public class WeatherCitiesResponse : Response
    {
        /// <summary>
        /// /List of all cities.. foreign plus Indian.
        /// </summary>
        private Dictionary<string, object> listOfCities;

        /// <summary>
        /// List of foreign cities.
        /// </summary>
        private Dictionary<string, object> ForeignCities;

        /// <summary>
        /// List of Indian cities.
        /// </summary>
        private Dictionary<string, object> indianCities;

        public WeatherCities cities;
         /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public WeatherCitiesResponse(string responseMessage)
            : base(responseMessage)
        {
            cities = new WeatherCities();
            cities.IndianCities = new List<string>();
            cities.ForeignCities = new List<string>();
            listOfCities = new Dictionary<string, object>();
            indianCities = new Dictionary<string, object>();
            ForeignCities = new Dictionary<string, object>();
            JavaScriptSerializerParse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        private void JavaScriptSerializerParse()
        {
            if (!string.IsNullOrEmpty(responseMessage))
            {
                JavaScriptSerializerTool serializer = new JavaScriptSerializerTool();
                listOfCities = serializer.DeserializeObject(responseMessage) as Dictionary<string, object>;
                indianCities = listOfCities["indian"] as Dictionary<string, object>;
                ForeignCities = listOfCities["world"] as Dictionary<string, object>;

                foreach (var key in indianCities.Keys)
                {
                    cities.IndianCities.Add(key);
                }

                foreach (var key in ForeignCities.Keys)
                {
                    cities.ForeignCities.Add(key);
                }
            }
        }
    }
}
