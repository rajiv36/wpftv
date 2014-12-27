
using JavaScriptSerializer.Serialization;
using System.Collections.Generic;
using NDTV.Controller;
namespace NDTV.Entities
{
    public class WeatherDetailsResponse :Response
    {
        /// <summary>
        /// /List of all cities.. foreign plus Indian.
        /// </summary>
        private Dictionary<string, object> cities;        

        /// <summary>
        /// Gets the city weather details
        /// </summary>
        public WeatherData CityWeatherData
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public WeatherDetailsResponse(string responseMessage)
            : base(responseMessage)
        {
            JSONParse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        private void JSONParse()
        {
            if (false == string.IsNullOrEmpty(responseMessage))
            {
                //weatherInformation = Utilities.Utility.Deserialize<WeatherRegion>(responseMessage);
                JavaScriptSerializerTool serializer = new JavaScriptSerializerTool();
                cities = serializer.DeserializeObject(responseMessage) as Dictionary<string, object>;
                if (cities.ContainsKey(ApplicationData.Settings.WeatherCountry))
                {
                    var localCities = cities[ApplicationData.Settings.WeatherCountry] as Dictionary<string, object>;
                    if (localCities.ContainsKey(ApplicationData.Settings.WeatherCity))
                    {
                        var cityDetails = localCities[ApplicationData.Settings.WeatherCity] as Dictionary<string, object>;
                        CityWeatherData = new WeatherData();
                        if (cityDetails.ContainsKey("city"))
                        {
                            CityWeatherData.CityName = cityDetails["city"] as string;
                        }
                        if (cityDetails.ContainsKey("temp_c"))
                        {
                            CityWeatherData.TemperatureCelsius = cityDetails["temp_c"] as string;
                        }
                        if (cityDetails.ContainsKey("image"))
                        {
                            CityWeatherData.Image = cityDetails["image"] as string;
                        }
                        if (cityDetails.ContainsKey("icon"))
                        {
                            CityWeatherData.Icon = cityDetails["icon"] as string;
                        }
                    }                
                }                         
            }
        }      
    }
}
