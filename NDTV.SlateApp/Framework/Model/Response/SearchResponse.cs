using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NDTV.Utilities;

namespace NDTV.Entities
{
    public class SearchResponse :Response
    {
        /// <summary>
        /// Search Type.
        /// </summary>
        public SearchType SearchResultType
        {
            get;
            private set;
        }

        /// <summary>
        /// Search Results.
        /// </summary>
        public ObservableCollection<Item> SearchResults
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="responseMessage">Response Message</param>
        /// <param name="searchType">Search Type</param>
        public SearchResponse(string responseMessage,SearchType searchType)
            : base(responseMessage)
        {
            this.SearchResultType = searchType;
            this.SearchResults = new ObservableCollection<Item>();

            switch (searchType)
            {
                case SearchType.Articles:
                    this.ArticleResultsParse();
                    break;
                case SearchType.Videos:
                    this.VideoResultsParse();
                    break;
                case SearchType.Photos:
                    this.PhotoResultsParse();
                    break;
            }
        }

        /// <summary>
        /// Parses the Articles List.
        /// </summary>
        public void ArticleResultsParse()
        {
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var relatedArticles = (from eachItem in element.Element("channel").Elements("item")
                                       select new TopStoryItem
                                       {
                                           Description = Helper.RemoveHtmlTags(!string.IsNullOrWhiteSpace(eachItem.Element("description").Value)?eachItem.Element("description").Value:string.Empty).Trim(),
                                           Title = Helper.RemoveHtmlTags(!string.IsNullOrWhiteSpace(eachItem.Element("title").Value)?eachItem.Element("title").Value:string.Empty),
                                           LinkArticle = new Uri(!string.IsNullOrWhiteSpace(eachItem.Element("link").Value.ToString())?eachItem.Element("link").Value.ToString():string.Empty),
                                           Guid = !string.IsNullOrWhiteSpace(eachItem.Element("guid").Value)?eachItem.Element("guid").Value:string.Empty,
                                           ImageLinkStatic = new Uri((!string.IsNullOrWhiteSpace(eachItem.Element("StoryImage").Value)) ? (eachItem.Element("StoryImage").Value) : (string.Empty),UriKind.RelativeOrAbsolute),
                                           LinkForSlate = new Uri((!string.IsNullOrWhiteSpace(eachItem.Element(@"permaLink").Element(@"device").Value) ? eachItem.Element(@"permaLink").Element(@"device").Value : string.Empty) + "?" + Utility.QueryStringForSlateDevice),
                                           LinkForRSS = new Uri(!string.IsNullOrWhiteSpace(eachItem.Element(@"permaLink").Element(@"rss").Value)?eachItem.Element(@"permaLink").Element(@"rss").Value:string.Empty),
                                           PublishedDate = Convert.ToDateTime(!string.IsNullOrWhiteSpace(eachItem.Element(@"pubDate").Value)?eachItem.Element(@"pubDate").Value:string.Empty,CultureInfo.InvariantCulture),
                                       }).ToList();
                for (int elementIndex = 0; elementIndex < relatedArticles.Count; elementIndex++)
                {
                    var currentElement = relatedArticles[elementIndex];
                    SearchResults.Add(currentElement);
                }
            }
        }

        /// <summary>
        /// Parses the Photos List.
        /// </summary>
        public void PhotoResultsParse()
        {
            int result;
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var relatedPhotos = (from eachItem in element.Element("channel").Elements("item")
                                   select new ImageAlbum
                                   {
                                       AlbumId = int.TryParse(eachItem.Element("id").Value, out result) ? result : -1,
                                       AlbumTitle = (false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("title").Value))) ? Helper.RemoveHtmlTags(eachItem.Element("title").Value) : string.Empty,
                                       AlbumDescription = (false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("description").Value))) ? Helper.RemoveHtmlTags(eachItem.Element("description").Value) : string.Empty,
                                       TotalImagesInAlbum = int.TryParse(eachItem.Element("TotalImages").Value, out result) ? result : -1,
                                       PublishedDataTimeOfAlbum = DateTime.Parse(eachItem.Element("pubDate").Value, CultureInfo.InvariantCulture),
                                       FolderPath = (false == string.IsNullOrWhiteSpace(eachItem.Element("folderPath").Value)) ? eachItem.Element("folderPath").Value : string.Empty,
                                       AlbumCoverImageName = (false == string.IsNullOrWhiteSpace(Helper.RemoveHtmlTags(eachItem.Element("image").Value))) ? Helper.RemoveHtmlTags(eachItem.Element("image").Value) : string.Empty,
                                       ThumbnailLink = (false == string.IsNullOrWhiteSpace(eachItem.Element("thumbnailUrl").Value)) ? eachItem.Element("thumbnailUrl").Value : string.Empty,
                                       AlbumLink = (false == string.IsNullOrWhiteSpace(eachItem.Element("albumurl").Value)) ? eachItem.Element("albumurl").Value : string.Empty
                                   }).ToList();
                for (int elementIndex = 0; elementIndex < relatedPhotos.Count; elementIndex++)
                {
                    var currentElement = relatedPhotos[elementIndex];
                    SearchResults.Add(currentElement);
                }
            }
        }

        /// <summary>
        /// Parses the Videos List.
        /// </summary>
        public void VideoResultsParse()
        {
            int result;
            XElement element = XElement.Parse(responseMessage);
            if (null != element && null != element.Elements())
            {
                var relatedVideos = (from eachItem in element.Element("channel").Elements("item")
                                     select new VideoItem
                                     {
                                         Title = (null != eachItem.Element("title")) ? eachItem.Element("title").Value.ToString() : string.Empty,
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
                for (int elementIndex = 0; elementIndex < relatedVideos.Count; elementIndex++)
                {
                    var currentElement = relatedVideos[elementIndex];
                    SearchResults.Add(currentElement);
                }
            }
        }

    }
}
