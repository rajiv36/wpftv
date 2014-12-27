using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;
using NDTV.Utilities;

namespace NDTV.SlateApp.ViewModel
{
    public class WeatherViewModel : ViewModelBase
    {
        private string weatherScreenLink;

        /// <summary>
        /// Weather Screen Link
        /// </summary>
        public string WeatherScreenLink
        {
            get { return weatherScreenLink; }
            set { weatherScreenLink = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WeatherViewModel()
        {
            WeatherScreenLink = Utility.GetLink(Constants.LinkNames.NDTVWeatherPageLink);
            ApplicationData.SetCurrentItem(Resources.TwitterWeatherTitle, string.Empty, string.Empty,
                                            string.Empty, WeatherScreenLink, ShareMediaType.Weather);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void DisposeResources()
        {
            WeatherScreenLink = null;
        }
    }
}
