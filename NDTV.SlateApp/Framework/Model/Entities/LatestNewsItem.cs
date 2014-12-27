using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    /// <summary>
    /// every property required for the LatestNews class would be mentioned here..
    /// all the properties bound in the ui should be present in this LatestNews class hierarchically..
    /// </summary>
    public class LatestNews : IEquatable<LatestNews>
    {
        public LatestNewsCollection latestNewsCollection { get; set; }
        public bool IsFlashNewsExist { get; set; }

        /// <summary>
        /// the place where LatestNews builds its object..
        /// </summary>
        /// <param name="responseMessage"></param>
        public void BuildGraphObject(string responseMessage)
        {
            XElement element;
            if (Utilities.Utility.XElementTryParse(responseMessage, out element) && element != null)
            {
                XElement channelElement = element.Element("channel");
                latestNewsCollection = new LatestNewsCollection();
                if (channelElement != null && channelElement.HasElements && channelElement.Element("item") != null)
                {
                    IEnumerable<LatestNewsItem> items = (from eachItem in channelElement.Elements("item")
                                                         select new LatestNewsItem(eachItem));
                    foreach (LatestNewsItem item in items) latestNewsCollection.Add(item);
                    IsFlashNewsExist = true;
                    return;
                }
            }
            IsFlashNewsExist = false;
        }

        /// <summary>
        /// The overrided Equals method which helps comparison
        /// </summary>
        /// <param name="other"></param>
        /// <returns>the Latest News section to be compared with the "this"(latestNews)</returns>
        public bool Equals(LatestNews other)
        {
            if (IsFlashNewsExist != other.IsFlashNewsExist)
                return false;
            else
            {
                if (null == latestNewsCollection || null == other.latestNewsCollection)
                {
                    return false;
                }
                else
                {
                    return latestNewsCollection.Equals(other.latestNewsCollection);
                }
            }
        }
    }

    /// <summary>
    /// LatestNews Item Collection which represents the item array which can be directly bound to any collection accepting control
    /// </summary>
    public class LatestNewsCollection : System.Collections.ObjectModel.Collection<LatestNewsItem>, IEquatable<LatestNewsCollection>
    {
        /// <summary>
        /// The overrided Equals method which helps comparison
        /// </summary>
        /// <param name="news"></param>
        /// <returns>the LatestNewsCollection section to be compared with the "this"(LatestNewsCollection)</returns>
        public bool Equals(LatestNewsCollection other)
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
    /// All the necessary for a single LatestNews item is found here
    /// </summary>
    public class LatestNewsItem : IEquatable<LatestNewsItem>
    {
        public int Id { get; set; }
        public string FlashNews { get; set; }
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// The method which constructs the object
        /// The instantiated object self-loads itself through this method
        /// </summary>
        /// <param name="item">the xmlElement associated with the LatestNewsSection</param>
        public LatestNewsItem(XElement item)
        {
            FlashNews = item.Element("title").Value;
            string publishedDateText = item.Element("pubDate").Value;            
            DateTime publishedDateValue;
            PublishedDate = string.IsNullOrEmpty(publishedDateText) ?
                new DateTime() : (DateTime.TryParse(publishedDateText,CultureInfo.InvariantCulture,DateTimeStyles.None,out publishedDateValue) ? publishedDateValue : new DateTime());
        }

        /// <summary>
        /// The overrided Equals method which helps comparison
        /// </summary>
        /// <param name="news"></param>
        /// <returns>the LatestNewsItem section to be compared with the "this"(LatestNewsItem)</returns>
        public bool Equals(LatestNewsItem other)
        {
            return (FlashNews == other.FlashNews) ? true : false;               
        }

    }
}
