using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    /// <summary>
    /// Response class for Image item request.
    /// </summary>
    public class ImageItemResponse : Response
    {
        /// <summary>
        /// List of Image Item.
        /// </summary>
        public List<ImageItem> ImageItemCollection { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        public ImageItemResponse(string responseMessage)
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
            DateTime resultDate;
            ImageItemCollection = new List<ImageItem>();
            var elementList = (from eachItem in element.Element("channel").Elements("content")
                               select new ImageItem()
                               {
                                   ImageId = (null != eachItem.Element("contentId"))?(int.TryParse(eachItem.Element("contentId").Value, out result)?result:-1):-1,
                                   ImageTitle = (null != eachItem.Element("contentId"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("contentTitle").Value)))?Helper.RemoveHtmlTags(eachItem.Element("contentTitle").Value):string.Empty):string.Empty,
                                   ImageShortDescription = (null != eachItem.Element("contentId"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("contentShortDesc").Value)))?Helper.RemoveHtmlTags(eachItem.Element("contentShortDesc").Value):string.Empty):string.Empty,
                                   ImageCompleteDescription = (null != eachItem.Element("contentId"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("contentDesc").Value)))?Helper.RemoveHtmlTags(eachItem.Element("contentDesc").Value):string.Empty):string.Empty,
                                   ImageCreatedDate = (null != eachItem.Element("contentId")) ? (DateTime.TryParse(eachItem.Element("contentCreatedDate").Value, CultureInfo.InvariantCulture, DateTimeStyles.None,out resultDate)?resultDate:DateTime.Now):DateTime.Now,
                                   ImageUpdatedDate = (null != eachItem.Element("contentId"))?(DateTime.TryParse(eachItem.Element("contentUpdatedDate").Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate)?resultDate:DateTime.Now):DateTime.Now,
                                   ImageSource = (null != eachItem.Element("contentId"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("contentSource").Value)))?Helper.RemoveHtmlTags(eachItem.Element("contentSource").Value):string.Empty):string.Empty,
                                   ImageThumbnailLink = (null != eachItem.Element("contentId"))?((false == string.IsNullOrWhiteSpace(eachItem.Element("contentThumbnailUrl").Value))?eachItem.Element("contentThumbnailUrl").Value:string.Empty):string.Empty,
                                   ImagePriority = (null != eachItem.Element("contentId"))?(int.TryParse(eachItem.Element("contentPriority").Value, out result) ? result : -1):-1
                               }).ToList();
            for (int elementIndex = 0; elementIndex < elementList.Count; elementIndex++)
            {
                var currentElement = elementList[elementIndex];
                if ((currentElement.ImagePriority == -1) || (currentElement.ImagePriority == 0))
                {
                    continue;
                }
                
                ImageItemCollection.Add(elementList.ElementAt(elementIndex));
            }
        }
    }
}
