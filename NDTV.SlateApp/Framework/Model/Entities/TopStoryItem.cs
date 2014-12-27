using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;
using System.Globalization;

namespace NDTV.Entities
{
    /// <summary>
    /// every property required for the TopStory class would be mentioned here..
    /// all the properties bound in the ui should be present in this TopStory class hierarchically..
    /// </summary>
    public class TopStory : IEquatable<TopStory>
    {
        public TopStory()
        {

        }
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string Language { get; set; }
        public DateTime LastBuildDate { get; set; }
        public TopStoryCollection TopStoryCollection { get; set; }

        /// <summary>
        /// The method which helps its own objects creation(Topstory gets loaded)
        /// </summary>
        /// <param name="xmlData"></param>
        public void BuildGraphObject(string xmlData)
        {
            XElement xmlNode = new XElement(XElement.Parse(xmlData).Element(@"channel"));
            Language = xmlNode.Element(@"language").Value;
            Link = new Uri(xmlNode.Element(@"link").Value);
            Title = xmlNode.Element(@"title").Value;
            DateTime publishedDateValue;
            LastBuildDate = ((null != xmlNode.Element(@"lastBuildDate")) ?
                (DateTime.TryParse(xmlNode.Element(@"lastBuildDate").Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedDateValue) ? publishedDateValue : new DateTime()) :
                DateTime.Now);

            IEnumerable<TopStoryItem> items = (from eachItem in xmlNode.Elements(@"item")
                                               select new TopStoryItem(eachItem));
            TopStoryCollection = new TopStoryCollection();
            foreach (TopStoryItem item in items)
            {
                TopStoryCollection.Add(item);
            }

        }

        /// <summary>
        /// The overrided Equals method which helps comparison
        /// </summary>
        /// <param name="news"></param>
        /// <returns>the TopStory section to be compared with the "this"(TopStory)</returns>
        public bool Equals(TopStory other)
        {
            return TopStoryCollection.Equals(other.TopStoryCollection);
        }

        /// <summary>
        /// Retrieves the first article with an image or if the entire list doesnot have any image then
        /// retrieve the first article from the list..
        /// </summary>
        /// <returns>First article</returns>
        public TopStoryItem RetrieveFirstArticle()
        {
            TopStoryItem firstImagedItem = null;
            if (null != this && null != TopStoryCollection && TopStoryCollection.Count > 0)
            {
                firstImagedItem = TopStoryCollection.First<TopStoryItem>(item => (null != item.ImageLinkStatic && 
                                                                                    false == string.IsNullOrWhiteSpace(item.ImageLinkStatic.ToString())));
                if (null == firstImagedItem)
                {
                    firstImagedItem = TopStoryCollection.First();
                }
            }
            return firstImagedItem;
        }
    }

    /// <summary>
    /// TopStory Item Collection which represents the item array which can be directly bound to any collection accepting control
    /// </summary>
    public class TopStoryCollection : System.Collections.ObjectModel.Collection<TopStoryItem>, IEquatable<TopStoryCollection>
    {
        /// <summary>
        /// The overrided Equals method which helps comparison
        /// </summary>
        /// <param name="news"></param>
        /// <returns>the TopStoryCollection section to be compared with the "this"(TopStoryCollection)</returns>
        public bool Equals(TopStoryCollection other)
        {
            if (base.Count != other.Count)
                return false;
            else
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (!base[i].Equals(other[i]))
                        return false;
                }
                return true;
            }
        }
    }
 
    /// <summary>
    /// All the properties required for a TopStory item is found here
    /// </summary>
    public class TopStoryItem:Item,IEquatable<TopStoryItem>
    {
        #region Initial Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public Uri LinkArticle { get; set; }
        public string Guid { get; set; }
        public Uri ImageLinkStatic { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }

        public bool IsSelected { get; set; }
        public Uri LinkForSlate { get; set; }
        public Uri LinkForRSS { get; set; }

        public bool IsLoaded { get; private set; }//a flag which says whether the news article has been loaded or not..

        #endregion

        #region Properties Set AfterMath by HTML Parsing 

        /// <summary>
        /// All the below mentioned 4 properties unused as of now,
        /// because we are directlyt binding the Arcticle URL to the browser
        /// Use the below properties if we bind the below properties instead of a URI.
        /// </summary>
        /*
        public Uri ImageLinkDynamic { get; set; }
        public string Story { get; set; }
        public DateTime UpdatedDate { get; set; }
        public TopStoryCollection TopStoryRelatedArticles { get; set; }//A property which is set to show the related News articles.. 
        */

        #endregion

        #region Constructor

        public TopStoryItem()
        {
        }

        public TopStoryItem(XElement item)
        {
            Description = ((null != item.Element("description")) ? Helper.RemoveHtmlTags(item.Element("description").Value).Trim() : string.Empty);
            Title = ((null != item.Element("title") ) ? Helper.RemoveHtmlTags(item.Element("title").Value) : string.Empty);
            LinkArticle = ((null != item.Element("link")) ? new Uri(item.Element("link").Value.ToString()) : null);
            Guid = ((null != item.Element("guid")) ? item.Element("guid").Value : string.Empty);
            ImageLinkStatic = ((string.IsNullOrWhiteSpace(item.Element("StoryImage").Value) ? null : new Uri(item.Element("StoryImage").Value)));
            if (null != item.Element(@"permaLink"))
            {       
                LinkForSlate = ((null != item.Element(@"permaLink").Element(@"device")) ? new Uri((item.Element(@"permaLink").Element(@"device").Value) + "?" + Utility.QueryStringForSlateDevice) : null);
                LinkForRSS = ((null != item.Element(@"permaLink").Element(@"rss")) ? new Uri(item.Element(@"permaLink").Element(@"rss").Value) : null);
            }
            DateTime publishedDateValue;
            PublishedDate = ((null != item.Element("pubDate")) ?
                (DateTime.TryParse(item.Element("pubDate").Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedDateValue) ? publishedDateValue : new DateTime()) :
                DateTime.Now);
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Comparison based on Title,LinkForIpad & Guid
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Equals(TopStoryItem other)
        {
            if (Title == other.Title &&LinkForSlate.ToString() == other.LinkForSlate.ToString() 
                 &&Guid == other.Guid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
