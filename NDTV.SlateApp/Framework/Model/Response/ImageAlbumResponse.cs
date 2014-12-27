using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;
using System.Net;
using System.IO;

namespace NDTV.Entities
{
    /// <summary>
    /// Response class for Image album request.
    /// </summary>
    public class ImageAlbumResponse : Response
    {
        /// <summary>
        /// List of albums of a particular category.
        /// </summary>
        public List<ImageAlbum> ImageAlbumCollection { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseMessage">Response message to be parsed</param>
        /// <param name="fetchHighResolutionImages">Flag indicating whether high resolution images are to be fetched</param>
        public ImageAlbumResponse(string responseMessage, bool fetchHighResolutionImages)
            : base(responseMessage)
        {
            Parse(fetchHighResolutionImages);
        }

        /// <summary>
        /// This method parses the response and creates the entities
        /// </summary>
        /// <param name="fetchHighResolutionImages">Flag indicating whether high resolution images are to be fetched</param>
        private void Parse(bool fetchHighResolutionImages)
        {
            XElement element = XElement.Parse(responseMessage);
            ImageAlbumCollection = new List<ImageAlbum>();
            int result;
            DateTime resultDate;
            var elementList = (from eachItem in element.Element("channel").Elements("item")
                               select new ImageAlbum()
                               {
                                   AlbumId = (null != eachItem.Element("id"))?(int.TryParse(eachItem.Element("id").Value,out result)?result:-1):-1,
                                   AlbumTitle = (null != eachItem.Element("title"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("title").Value))) ? Helper.RemoveHtmlTags(eachItem.Element("title").Value) : string.Empty):string.Empty,
                                   AlbumDescription = (null != eachItem.Element("description"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("description").Value))) ? Helper.RemoveHtmlTags(eachItem.Element("description").Value) : string.Empty):string.Empty,
                                   TotalImagesInAlbum = (null != eachItem.Element("TotalImages"))?(int.TryParse(eachItem.Element("TotalImages").Value,out result)?result:-1):-1,
                                   PublishedDataTimeOfAlbum = (null != eachItem.Element("pubDate"))?(DateTime.TryParse(eachItem.Element("pubDate").Value,CultureInfo.InvariantCulture,DateTimeStyles.None,out resultDate)?resultDate:DateTime.Now):DateTime.Now,
                                   FolderPath = (null != eachItem.Element("folderPath"))?((false == string.IsNullOrWhiteSpace(eachItem.Element("folderPath").Value))?eachItem.Element("folderPath").Value:string.Empty):string.Empty,
                                   AlbumCoverImageName = (null != eachItem.Element("image"))?((false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("image").Value)))?Helper.RemoveHtmlTags(eachItem.Element("image").Value):string.Empty):string.Empty,
                                   ThumbnailLink = (null != eachItem.Element("thumbnailUrl")) ? ((false == string.IsNullOrWhiteSpace(eachItem.Element("thumbnailUrl").Value)) ? CheckIfImageExits(eachItem.Element("thumbnailUrl").Value,fetchHighResolutionImages) : string.Empty) : string.Empty,
                                   AlbumLink = (null != eachItem.Element("albumurl"))?((false == string.IsNullOrWhiteSpace(eachItem.Element("albumurl").Value))?eachItem.Element("albumurl").Value:string.Empty):string.Empty
                               }).ToList();
            for (int elementIndex = 0; elementIndex < elementList.Count; elementIndex++)
            {
                var currentElement = elementList[elementIndex];
                
                ImageAlbumCollection.Add(elementList.ElementAt(elementIndex));
            }
        }

        /// <summary>
        /// web client to check whether Image link exixts or not.
        /// </summary>
        private WebClient client = null;

        /// <summary>
        /// new Thumbnail link.
        /// </summary>
        private string newThumbnailLink = null;
        
        /// <summary>
        /// Checks whether image exists
        /// </summary>
        /// <param name="thumbnailLink">Thumbnail URL</param>
        /// <returns>Thumbnail URL</returns>
        private string CheckIfImageExits(string thumbnailLink, bool fetchHighResolutionImage)
        {
            if (false == fetchHighResolutionImage)
            {
                return thumbnailLink;
            }
            else
            {

                try
                {
                    newThumbnailLink = thumbnailLink.Replace(Constants.Constant.JpgExtension, Constants.Constant.NewHighResolutionJpgExtension);
                    client = new WebClient();
                    string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Utility.ApplicationFolderPath);
                    if (false == Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string tempImagePath = Path.Combine(directoryPath, (Constants.Constant.Image + Guid.NewGuid() + Constants.Constant.JpgExtension));
                    client.DownloadFile(newThumbnailLink, tempImagePath);
                    File.Delete(tempImagePath);
                    client = null;
                    return newThumbnailLink;
                }
                catch (WebException ex)
                {
                    return thumbnailLink;
                }
            }

        }
    }
}
