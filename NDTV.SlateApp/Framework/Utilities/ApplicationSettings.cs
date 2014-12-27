using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NDTV.Utilities
{   
    /// <summary>
    /// This class contains the application settings
    /// </summary>
    [Serializable]
    public class ApplicationSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationSettings()
        {
            //TODO: Move these values to constants
            WeatherCountry = "indian";
            WeatherCity = "Delhi";
        }
    

        /// <summary>
        /// Gets and sets the last browsed news category
        /// </summary>
        [XmlElement("LastBrowsedNewsCategory")]
        public string NewsCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the last viewed TV channel
        /// </summary>
        [XmlElement("LastViewedLiveTVChannel")]
        public string LiveTVChannel
        {
            get;
            set;
        }

        // <summary>
        /// Gets and sets the last browsed news category
        /// </summary>
        [XmlElement("LastBrowsedImageAlbumCarouselCategory")]
        public string ImageAlbumCarouselCategory
        {
            get;
            set;
        }

        // <summary>
        /// Gets and sets the last browsed news category
        /// </summary>
        [XmlElement("LastBrowsedVideoAlbumCarouselCategory")]
        public string VideoAlbumCarouselCategory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the last selected weather country
        /// </summary>
        [XmlElement("LastSelectedWeatherCountry")]
        public string WeatherCountry
        {
            get;
            set;
        }


        // <summary>
        /// Gets and sets the last selected weather city.
        /// </summary>
        [XmlElement("LastSelectedWeatherCity")]
        public string WeatherCity
        {
            get;
            set;
        }

        // <summary>
        /// Gets and sets the last browsed news category
        /// </summary>
        [XmlElement("UnsavedFeedback")]
        public string UnsavedFeedback
        {
            get;
            set;
        }


    }
}
