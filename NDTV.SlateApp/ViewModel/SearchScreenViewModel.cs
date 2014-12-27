using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NDTV.Entities;
using NDTV.SlateApp.Framework.CustomEventArgs;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// Search Screen View Model class to interact with the Search Screen View.
    /// </summary>
    public class SearchScreenViewModel : ViewModelBase
    {
        #region Events

        public event EventHandler<SearchCompletedEventArgs> SearchCompletedEvent;

        #endregion

        #region Private Fields

        /// <summary>
        /// Flag to indicate if more news articles can be fetched
        /// </summary>
        private bool canFetchMoreNewsArticles;

        /// <summary>
        /// Flag to indicate if more photos can be fetched
        /// </summary>
        private bool canFetchMorePhotos;

        /// <summary>
        /// Flag to indicate of more videos can be fetched
        /// </summary>
        private bool canFetchMoreVideos;

        /// <summary>
        /// Variable to control the preloader in the News Articles Container 
        /// </summary>
        private bool isNewsArticleContainerBusy;

        /// <summary>
        /// Variable to control the preloader in the Video Container
        /// </summary>
        private bool isVideoContainerBusy;

        /// <summary>
        /// Variable to control the preloader in the Photo Container
        /// </summary>
        private bool isPhotoContainerBusy;

        /// <summary>
        /// Page Number of the current News Article List
        /// </summary>
        private int currentNewsArticlePageNumber;

        /// <summary>
        /// Pgae Number of the current Photo Gallery List
        /// </summary>
        private int currentPhotoGalleryPageNumber;

        /// <summary>
        /// Page Number of the current Video Gallery List
        /// </summary>
        private int currentVideoGalleryPageNumber;

        /// <summary>
        /// Command invoked when the search button is clicked.
        /// </summary>
        private RelayCommand onSearchButtonClickCommand;

        /// <summary>
        /// Command invoked when the concerned ScrollViewer reaches the end of the listbox.
        /// </summary>
        private RelayCommand fetchMoreDataCommand;

        /// <summary>
        /// The Search String.
        /// </summary>
        private string searchString;

        /// <summary>
        /// Quoted search string
        /// </summary>
        private string quotedSearchString;


        #endregion

        #region Public Properties

        /// <summary>
        /// Variable to control the preloader in the News Articles Container 
        /// </summary>
        public bool IsNewsArticleContainerBusy
        {
            get { return isNewsArticleContainerBusy; }
            set
            {
                isNewsArticleContainerBusy = value;
                OnPropertyChanged("IsNewsArticleContainerBusy");
            }
        }

        /// <summary>
        /// Variable to control the preloader in the Video Container
        /// </summary>
        public bool IsVideoContainerBusy
        {
            get { return isVideoContainerBusy; }
            set
            {
                isVideoContainerBusy = value;
                OnPropertyChanged("IsVideoContainerBusy");
            }
        }

        /// <summary>
        /// Variable to control the preloader in the Photo Container
        /// </summary>
        public bool IsPhotoContainerBusy
        {
            get { return isPhotoContainerBusy; }
            set
            {
                isPhotoContainerBusy = value;
                OnPropertyChanged("IsPhotoContainerBusy");
            }
        }

        /// <summary>
        /// Collection to store the Results retrieved for the Photo Search.
        /// </summary>
        public ObservableCollection<ImageAlbum> PhotoSearchResults { get; private set; }

        /// <summary>
        /// Collection to store the Results retrieved for the Video Search.
        /// </summary>
        public ObservableCollection<VideoItem> VideoSearchResults { get; private set; }

        /// <summary>
        /// Collection to store the Results retrieved for the News Articles Search.
        /// </summary>
        public ObservableCollection<TopStoryItem> NewsSearchResults { get; private set; }

        /// <summary>
        /// Command invoked when the search button is clicked.
        /// </summary>
        public RelayCommand OnSearchButtonClickCommand
        {
            get
            {
                if (null == onSearchButtonClickCommand)
                {
                    onSearchButtonClickCommand = new RelayCommand((e) => OnSearchButtonClick(e));
                }
                return onSearchButtonClickCommand;
            }
        }

        /// <summary>
        /// Command invoked when the scroll viewer in the listbox reaches the end.
        /// </summary>
        public RelayCommand FetchMoreDataCommand
        {
            get
            {
                if (null == this.fetchMoreDataCommand)
                {
                    this.fetchMoreDataCommand = new RelayCommand((e) => FetchMoreData((SearchType)(e)));
                }
                return fetchMoreDataCommand;
            }
        }

        /// <summary>
        /// The Search String.
        /// </summary>
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                QuotedSearchString = "'" + value + "'";
                OnPropertyChanged("SearchString");
            }
        }

        /// <summary>
        /// Search String embedded in quotes
        /// </summary>
        public string QuotedSearchString
        {
            get
            {
                return quotedSearchString;
            }
            set
            {
                quotedSearchString = value;
                OnPropertyChanged("QuotedSearchString");
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchScreenViewModel()
        {
            this.VideoSearchResults = new ObservableCollection<VideoItem>();
            this.PhotoSearchResults = new ObservableCollection<ImageAlbum>();
            this.NewsSearchResults = new ObservableCollection<TopStoryItem>();
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Function that loads the appropriate collection with the concerned Search Results.
        /// </summary>
        /// <param name="response">Response</param>
        public void LoadSearchResults(Response response)
        {
            SearchType type;
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (null != response)
                {
                    var responseCast = (response as SearchResponse);
                    if (null != responseCast)
                    {
                        type = responseCast.SearchResultType;
                        var results = responseCast.SearchResults;
                        if (null != type && null != results)
                        {
                            switch (type)
                            {
                                case SearchType.Articles:
                                    foreach (var newsItem in results)
                                    {
                                        if (null != this.NewsSearchResults)
                                        {
                                            this.NewsSearchResults.Add(newsItem as TopStoryItem);
                                        }
                                    }
                                    SearchCompletedEvent(this, new SearchCompletedEventArgs() { SearchKind = SearchType.Articles, SearchResults = new List<Item>(this.NewsSearchResults.ToList()) });
                                    break;
                                case SearchType.Photos:
                                    foreach (var photoItem in results)
                                    {
                                        if (null != this.PhotoSearchResults)
                                        {
                                            this.PhotoSearchResults.Add(photoItem as ImageAlbum);
                                        }
                                    }
                                    SearchCompletedEvent(this, new SearchCompletedEventArgs() { SearchKind = SearchType.Photos, SearchResults = new List<Item>(this.PhotoSearchResults.ToList()) });
                                    break;
                                case SearchType.Videos:
                                    foreach (var videoItem in results)
                                    {
                                        if (null != this.VideoSearchResults)
                                        {
                                            this.VideoSearchResults.Add(videoItem as VideoItem);
                                        }
                                    }
                                    SearchCompletedEvent(this, new SearchCompletedEventArgs() { SearchKind = SearchType.Videos, SearchResults = new List<Item>(this.VideoSearchResults.ToList()) });
                                    break;
                            }
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// Function invoked when the search button is clicked
        /// </summary>
        private void OnSearchButtonClick(object searchText)
        {
            this.SearchString = searchText.ToString();
            if (false==string.IsNullOrWhiteSpace(this.SearchString))
            {
                InitializeCollections();
                this.IsNewsArticleContainerBusy = true;
                this.IsPhotoContainerBusy = true;
                this.IsVideoContainerBusy = true;
                this.currentNewsArticlePageNumber = 1;
                this.currentPhotoGalleryPageNumber = 1;
                this.currentVideoGalleryPageNumber = 1;
                this.canFetchMoreNewsArticles = true;
                this.canFetchMorePhotos = true;
                this.canFetchMoreVideos = true;
                ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.VideoSearchLink), this.SearchString,this.currentVideoGalleryPageNumber, SearchType.Videos), LoadSearchResults);
                ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.PhotoSearchLink), this.SearchString,this.currentPhotoGalleryPageNumber, SearchType.Photos), LoadSearchResults);
                ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.NewsSearchLink), this.SearchString,this.currentNewsArticlePageNumber, SearchType.Articles), LoadSearchResults);
            }
        }

        /// <summary>
        /// Method to initialize Collections
        /// </summary>
        private void InitializeCollections()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (null != this.NewsSearchResults)
                {
                    if (this.NewsSearchResults.Count > 0)
                    {
                        if (null != this.NewsSearchResults)
                        {
                            this.NewsSearchResults.Clear();
                        }
                    }
                }
                else
                {
                    this.NewsSearchResults = new ObservableCollection<TopStoryItem>();
                }

                if (null != this.PhotoSearchResults)
                {
                    if (this.PhotoSearchResults.Count > 0)
                    {
                        if (null != this.PhotoSearchResults)
                        {
                            this.PhotoSearchResults.Clear();
                        }
                    }
                }
                else
                {
                    this.PhotoSearchResults = new ObservableCollection<ImageAlbum>();
                }

                if (null != this.VideoSearchResults)
                {
                    if (this.VideoSearchResults.Count > 0)
                    {
                        if (null != this.VideoSearchResults)
                        {
                            this.VideoSearchResults.Clear();
                        }
                    }
                }
                else
                {
                    this.VideoSearchResults = new ObservableCollection<VideoItem>();
                }
            }));
            
        }

        /// <summary>
        /// Function which makes the call and fetches more data.
        /// </summary>
        /// <param name="selectedCategory">The selected category</param>
        private void FetchMoreData(SearchType selectedCategory)
        {
            switch (selectedCategory)
            {
                case SearchType.Articles:
                    if (true == this.canFetchMoreNewsArticles)
                    {
                        this.currentNewsArticlePageNumber += 1;
                        ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.NewsSearchLink), this.SearchString, this.currentNewsArticlePageNumber, SearchType.Articles), LoadAdditionalSearchResults);
                    }
                    break;
                case SearchType.Photos:
                    if (true == this.canFetchMorePhotos)
                    {
                        this.currentPhotoGalleryPageNumber += 1;
                        ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.PhotoSearchLink), this.SearchString, this.currentPhotoGalleryPageNumber, SearchType.Photos), LoadAdditionalSearchResults);
                    }
                    break;
                case SearchType.Videos:
                    if (true == this.canFetchMoreVideos)
                    {
                        this.currentVideoGalleryPageNumber += 1;
                        ProcessRequest(new SearchRequest(Utilities.Utility.GetLink(Constants.LinkNames.VideoSearchLink), this.SearchString, this.currentVideoGalleryPageNumber, SearchType.Videos), LoadAdditionalSearchResults);
                    }
                    break;
            }
        }

        /// <summary>
        /// Response method which adds more data into the existing set of results.
        /// </summary>
        /// <param name="response">Response</param>
        private void LoadAdditionalSearchResults(Response response)
        {
            if (null != response)
            {
                var cast = (response as SearchResponse);
                if (null != cast)
                {
                    var resultsType = cast.SearchResultType;
                    var additionalSearchResults = cast.SearchResults;
                    switch (resultsType)
                    {
                        case SearchType.Articles:
                            if (null != additionalSearchResults && additionalSearchResults.Count > 0)
                            {
                                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                                {
                                    foreach (var item in additionalSearchResults)
                                    {
                                        if (null != this.NewsSearchResults)
                                        {
                                            this.NewsSearchResults.Add(item as TopStoryItem);
                                        }
                                    }
                                }));
                            }
                            else
                            {
                                this.canFetchMoreNewsArticles = false;
                            }
                            break;
                        case SearchType.Photos:
                            if (null != additionalSearchResults && additionalSearchResults.Count > 0)
                            {
                                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                                {
                                    foreach (var item in additionalSearchResults)
                                    {
                                        if (null != this.PhotoSearchResults)
                                        {
                                            this.PhotoSearchResults.Add(item as ImageAlbum);
                                        }
                                    }
                                }));
                            }
                            else
                            {
                                this.canFetchMorePhotos = false;
                            }
                            break;
                        case SearchType.Videos:
                            if (null != additionalSearchResults && additionalSearchResults.Count > 0)
                            {
                                App.Current.Dispatcher.BeginInvoke(new Action(delegate()
                                {
                                    foreach (var item in additionalSearchResults)
                                    {
                                        if (null != this.VideoSearchResults)
                                        {
                                            this.VideoSearchResults.Add(item as VideoItem);
                                        }
                                    }
                                }));
                            }
                            else
                            {
                                this.canFetchMoreVideos = false;
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void DisposeResources()
        {
            PhotoSearchResults = null;
            VideoSearchResults = null;
            NewsSearchResults = null;
            onSearchButtonClickCommand = null;
            fetchMoreDataCommand = null;
        }

        #endregion
    }

}
