using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NDTV.Entities
{
    /// <summary>
    /// Categories feed response class
    /// </summary>
    public class CategoriesFeedResponse : Response
    {
        public Category Category { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public CategoriesFeedResponse(string responseMessage)
            : base(responseMessage)
        {
            Category = new Category();
            Parse();
        }

        /// <summary>
        /// This method parses the response and creates the entities
        /// </summary>
        private void Parse()
        {
            XElement element = XElement.Parse(responseMessage);
            int result;
            var elementList = (from eachItem in element.Element("channel").Elements("item")
                               select new CategoryItem()
                                {
                                    Title = eachItem.Element("title").Value,
                                    Link = eachItem.Element("link").Value,
                                    Priority = int.TryParse(eachItem.Element("priority").Value,out result) ? result : -1,
                                    IsNDTVSourceCategory = true
                                }).ToList();
            for (int elementIndex = 0; elementIndex < elementList.Count; elementIndex++)
            {
                var currentElement = elementList[elementIndex];
                if (currentElement.Priority == -1)
                {
                    continue;
                }
                Category.categoryItemList.Add(currentElement.Priority, currentElement);
            }
        }
    }
}
