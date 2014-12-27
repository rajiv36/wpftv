using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    public class ProfitFlashNewsItemResponse :Response
    {

        /// <summary>
        /// The appropriate News Flash Item.
        /// </summary>
        public ProfitFlashNewsItem NewsflashItem { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response</param>
        public ProfitFlashNewsItemResponse(string responseMessage)
            :base(responseMessage)
        {
            this.NewsflashItem = new ProfitFlashNewsItem();
            Parse();
        }

        /// <summary>
        /// Parse the response and fill the appropriate model.
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);

            var newsFlashItem = (from eachItem in element.Elements("channel")
                                   select new ProfitFlashNewsItem
                                   {
                                       Title = (null != eachItem.Element("title")) ? eachItem.Element("title").Value.ToString() : string.Empty,
                                       Description = (null != eachItem.Element("description")) ? eachItem.Element("description").Value.ToString() : string.Empty,
                                       Language = (null != eachItem.Element("language")) ? eachItem.Element("language").Value.ToString() : string.Empty,
                                       ItemDetails = (from eachItemDetails in element.Elements("channel").Elements("item")
                                                      select new ProfitFlashNewsItemDetails
                                                      {
                                                          FlashItemDetails = (null != eachItemDetails.Element("title")) ? eachItemDetails.Element("title").Value.ToString() : string.Empty,
                                                          Date = (null != eachItemDetails.Element("pubDate")) ? eachItemDetails.Element("pubDate").Value.ToString() : string.Empty,
                                                          FlashNewsType = (null != eachItemDetails.Element("type")) ? eachItemDetails.Element("type").Value.ToString() : string.Empty
                                                      }).ToList()
                                   }).FirstOrDefault();

            this.NewsflashItem = newsFlashItem;

        }
    }
}
