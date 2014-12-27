using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Response class for Image categories request.
    /// </summary>
    public class ImageCategoriesResponse : Response
    {
        /// <summary>
        /// List of categories available like Sports/Entertainment/LifeStyle etc.
        /// </summary>
        public List<ImageCategoryItem> ImageCategoryCollection { get; private set; }

         /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public ImageCategoriesResponse(string responseMessage)
            : base(responseMessage)
        {
            Parse();
        }

        /// <summary>
        /// This method parses the response and creates the entities
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);
            int result;
            ImageCategoryCollection = new List<ImageCategoryItem>();
            var elementList = (from eachItem in element.Element("channel").Elements("item")
                               select new ImageCategoryItem()
                               {
                                   ImageAlbumTitle = (null != eachItem.Element("title"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("title").Value)))?Helper.RemoveHtmlTags(eachItem.Element("title").Value):string.Empty):string.Empty,
                                   ImageAlbumLink = (null != eachItem.Element("link"))?((false == string.IsNullOrWhiteSpace(eachItem.Element("link").Value))?eachItem.Element("link").Value:string.Empty):string.Empty,
                                   Priority = (null != eachItem.Element("priority"))?(int.TryParse(eachItem.Element("priority").Value, out result) ? result : -1):-1
                               }).ToList();
            for (int elementIndex = 0; elementIndex < elementList.Count; elementIndex++)
            {
                var currentElement = elementList[elementIndex];
                if (currentElement.Priority == -1)
                {
                    continue;
                }

                ImageCategoryCollection.Add(elementList.ElementAt(elementIndex));
            }
        }
    }
}
