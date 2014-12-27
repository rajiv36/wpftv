using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Response for related videos
    /// </summary>
    public class RelatedVideosResponse : Response
    {
        /// <summary>
        /// Constructor which passes the response to base class constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public RelatedVideosResponse(string responseMessage)
            : base(responseMessage)
        {
            relatedVideoList = new ObservableCollection<VideoItem>();
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
                var relatedVideos = (from eachItem in element.Element("channel").Elements("item")
                                     select new VideoItem
                                     {
                                         Title = (null != eachItem.Element("title")) ? Helper.RemoveHtmlTags(eachItem.Element("title").Value.ToString()) : string.Empty,
                                         VideoLink = (null != eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "ndtv_permalink")) ? eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "ndtv_permalink").Value.ToString() : string.Empty,
                                         Description = (null != eachItem.Element("description")) ? Helper.RemoveHtmlTags((eachItem.Element("description").Value.ToString())) : string.Empty,
                                         VideoFilePath = (null != eachItem.Element("filepath")) ? eachItem.Element("filepath").Value.ToString() : string.Empty,
                                         VideoId = (null != eachItem.Element("videoId")) ? (int.TryParse(eachItem.Element("videoId").Value, out result) ? result : -1) : -1,
                                         ThumbnailLink = (null != eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "thumbnail")) ?
                                                    eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "thumbnail").Attribute("url").Value.ToString() : string.Empty,
                                         ThumbnailLinkLarge = (null != eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "fullimage")) ?
                                                    eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "fullimage").Attribute("url").Value.ToString() : string.Empty,
                                         PublishDate = (null != eachItem.Element("pubDate")) ? eachItem.Element("pubDate").Value.ToString() : string.Empty,
                                         Duration = (null != eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "duration")) ? eachItem.Element(eachItem.GetNamespaceOfPrefix("media") + "duration").Value.ToString() : string.Empty

                                     }).ToList();
                foreach (VideoItem videos in relatedVideos)
                {
                    relatedVideoList.Add(videos);
                }
            }
        }

        private Collection<VideoItem> relatedVideoList;

        /// <summary>
        /// Property to get list of related videos for category
        /// </summary>
        public Collection<VideoItem> RelatedVideoList
        {
            get { return relatedVideoList; }
            set { relatedVideoList = value; }
        }

    }
}
