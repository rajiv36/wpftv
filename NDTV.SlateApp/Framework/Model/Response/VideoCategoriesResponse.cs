using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    /// <summary>
    /// Class to hold response for Video category list
    /// </summary>
    public class VideoCategoriesResponse : Response
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public VideoCategoriesResponse(string responseMessage)
            : base(responseMessage)
        {
            videoCategoryList = new ObservableCollection<VideoListCategories>();
            Parse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model
        /// </summary>
        private void Parse()
        {
            int result;
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var videoList = (from eachItem in element.Element("channel").Elements("item")
                                 select new VideoListCategories
                                 {
                                     Link = (null != eachItem.Element("link")) ? eachItem.Element("link").Value.ToString() : string.Empty,
                                     ImageLink = (null != eachItem.Element("image")) ? eachItem.Element("image").Value.ToString() : string.Empty,
                                     CategoryTitle = (null != eachItem.Element("title")) ? eachItem.Element("title").Value.ToString() : string.Empty,
                                     Priority = (null != eachItem.Element("priority")) ? (int.TryParse(eachItem.Element("priority").Value, out result) ? result : -1) : -1,
                                 }).ToList();
                for (int elementIndex = 0; elementIndex < videoList.Count; elementIndex++)
                {
                    var currentElement = videoList[elementIndex];
                    videoCategoryList.Add(currentElement);
                }
            }
        }

        private ObservableCollection<VideoListCategories> videoCategoryList;

        /// <summary>
        /// Gets the video category list
        /// </summary>
        public ObservableCollection<VideoListCategories> VideoCategoryList
        {
            get
            {
                return videoCategoryList;
            }
        }
    }
}
