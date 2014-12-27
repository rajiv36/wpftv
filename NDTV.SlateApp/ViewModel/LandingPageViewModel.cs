using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Input;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Properties;
using NDTV.Utilities;
using System.IO;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// View model class for landing page
    /// </summary>
    public class LandingPageViewModel : ViewModelBase
    {

        /// <summary>
        /// The Category item that remains selected at the user interface level..
        /// and the default being TopStories
        /// </summary>
        private CategoryItem activeselectedCategoryItem;
        public CategoryItem ActiveSelectedCategoryItem
        {
            get
            {
                if (null != Category && null != Category.CategoryItemList && Category.CategoryItemList.Count > 0)
                {
                    return ((null == activeselectedCategoryItem) ?
                        Category.CategoryItemList.Where(item => (item.Title.ToUpperInvariant() ==
                            Constants.TopStories.Categories.TopStory.ToUpperInvariant())).First() :
                        activeselectedCategoryItem);
                    //return Top_Story Category if there is no selected category present
                }
                else
                {
                    return null;
                }
            }
            set
            {
                activeselectedCategoryItem = value;
                if (null != value)
                {
                    ApplicationData.Settings.NewsCategory = activeselectedCategoryItem.Title;
                }
                OnPropertyChanged("activeselectedCategoryItem");
            }
        }

        /// <summary>
        /// TopStories data contract
        /// </summary>
        private TopStory topStories;
        public TopStory TopStories
        {
            get 
            { 
                return topStories; 
            }
            set
            {
                if (null != topStories && null != value && value.Equals(topStories))
                { 
                    return; 
                }
                topStories = value;
                OnPropertyChanged("TopStories");
            }
        }

        // We have split the source containers separately for Landscape and Potriat version as they both have different formats.
        /// <summary>
        /// TopStories data contract Container 1 which binds to the FIRST column of the main page
        /// </summary>
        private ObservableCollection<TopStoryItem> topStoriesContainer1 = new ObservableCollection<TopStoryItem>();
        public ObservableCollection<TopStoryItem> TopStoriesContainer1
        {
            get 
            { 
                return topStoriesContainer1; 
            }
            set
            {
                if (null != topStoriesContainer1 && null != value && value.Equals(topStoriesContainer1))
                {
                    return;
                }
                topStoriesContainer1 = value;
                OnPropertyChanged("TopStoriesContainer1");
            }
        }

        /// <summary>
        /// TopStories data contract Container 2 which binds to the SECOND column of the main page
        /// </summary>
        private ObservableCollection<TopStoryItem> topStoriesContainer2 = new ObservableCollection<TopStoryItem>();
        public ObservableCollection<TopStoryItem> TopStoriesContainer2
        {
            get 
            { 
                return topStoriesContainer2; 
            }
            set
            {
                if (null != topStoriesContainer2 && null != value && value.Equals(topStoriesContainer2))
                {
                    return;
                }
                topStoriesContainer2 = value;
                OnPropertyChanged("TopStoriesContainer2");
            }
        }


        /// <summary>
        /// TopStories data contract Container 1 which gets bound to the FIRST column of the POTRIAT Version of the main page
        /// </summary>
        private TopStoryItem topStoriesContainer3 = new TopStoryItem();
        public TopStoryItem TopStoriesContainer3
        {
            get 
            { 
                return topStoriesContainer3; 
            }
            set
            {
                if (null != topStoriesContainer3 && null != value && value.Equals(topStoriesContainer3))
                {
                    return;
                }
                topStoriesContainer3 = value;
                OnPropertyChanged("TopStoriesContainer3");
            }
        }

        /// <summary>
        /// TopStories data contract Container 2 which contains one article.
        /// </summary>
        private ObservableCollection<TopStoryItem> topStoriesContainer4 = new ObservableCollection<TopStoryItem>();
        public ObservableCollection<TopStoryItem> TopStoriesContainer4
        {
            get 
            { 
                return topStoriesContainer4; 
            }
            set
            {
                if (null != topStoriesContainer4 && null != value && value.Equals(topStoriesContainer4))
                {
                    return;
                }
                topStoriesContainer4 = value;
                OnPropertyChanged("TopStoriesContainer4");
            }
        }



        /// <summary>
        /// LatestNews data contract
        /// </summary>
        private LatestNews latestNews;
        public LatestNews LatestNews
        {
            get 
            { 
                return latestNews; 
            }
            set
            {
                if (null != latestNews && null != value && value.Equals(latestNews))
                {
                    return;
                }
                latestNews = value;
                OnPropertyChanged("LatestNews");
            }
        }

        /// <summary>
        /// Photo Albums Collection
        /// </summary>
        private List<ImageAlbum> photoAlbums;
        public List<ImageAlbum> PhotoAlbums
        {
            get 
            { 
                return photoAlbums; 
            }
            set
            {
                photoAlbums = value;
                OnPropertyChanged("PhotoAlbums");
            }
        }

        /// <summary>
        /// Category data contract
        /// </summary>
        private Category category;
        public Category Category
        {
            get 
            { 
                return category; 
            }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        /// <summary>
        /// Landing Page Loaded completely associated flag..
        /// This flag plays a vital role in switching on/off the preloader
        /// </summary>
        private bool isLandingPageLoaded;
        public bool IsLandingPageLoaded
        {
            get
            {
                return isLandingPageLoaded;
            }
            set
            {
                isLandingPageLoaded = value;
                OnPropertyChanged("IsLandingPageLoaded");
            }
        }

        /// <summary>
        /// Bool value to indicate if the search pop up is open/not
        /// </summary>
        private bool isSearchPopupOpen;

        /// <summary>
        /// Bool value to indicate if the search pop up is open/not
        /// </summary>
        public bool IsSearchPopupOpen
        {
            get { return isSearchPopupOpen; }
            set 
            { 
                isSearchPopupOpen = value;
                OnPropertyChanged("IsSearchPopupOpen");
            }
        }

        /// <summary>
        /// Search String
        /// </summary>
        private string searchString;

        /// <summary>
        /// Search String
        /// </summary>
        public string SearchString
        {
            get { return searchString; }
            set 
            { 
                searchString = value;
                OnPropertyChanged("SearchString");
            }
        }
        /// <summary>
        /// A collection which is retrieved by the Related Video Request class..
        /// </summary>
        private ObservableCollection<VideoItem> videoItems;
        public ObservableCollection<VideoItem> VideoItems
        {
            get
            {
                return videoItems;
            }
            set
            {
                videoItems = value;
                OnPropertyChanged("VideoItems");
            }
        }

        //Supporting Properties

        /// <summary>
        /// Profit Index data contract
        /// </summary>
        private StockIndexItem profitIndices;
        public StockIndexItem ProfitIndices
        {
            get 
            { 
                return profitIndices;
            }
            set
            {
                profitIndices = value;
                OnPropertyChanged("ProfitIndices");
            }
        }

        /// <summary>
        /// landing page relay command to be used to bind the category items on the dropdown..
        /// </summary>
        private RelayCommand landingPageMenuItemRelayCommander;
        public ICommand LandingPageMenuItemRelayCommander
        {
            get
            {
                if (null == landingPageMenuItemRelayCommander)
                {
                    landingPageMenuItemRelayCommander = new RelayCommand(ExecuteLandingPageMenuItemCommand);
                }
                return landingPageMenuItemRelayCommander;
            }
        }

        /// <summary>
        /// landing page relay command to be used to bind extra data as the user scrolls down..
        /// </summary>
        private RelayCommand landingPageTopStoryItemsRelayCommander;
        public ICommand LandingPageTopStoryItemsRelayCommander
        {
            get
            {
                if (null == landingPageTopStoryItemsRelayCommander)
                {
                    landingPageTopStoryItemsRelayCommander = new RelayCommand(CollectionHandler);
                }
                return landingPageTopStoryItemsRelayCommander;
            }
        }

        /// <summary>
        /// landing page relay command to be used for flipping between categories
        /// </summary>
        private RelayCommand landingPageCategoryFlipRelayCommander;
        public ICommand LandingPageCategoryFlipRelayCommander
        {
            get
            {
                if (null == landingPageCategoryFlipRelayCommander)
                {
                    landingPageCategoryFlipRelayCommander = new RelayCommand(CategoryFlipUnit);
                }
                return landingPageCategoryFlipRelayCommander;
            }
        }

        /// <summary>
        /// landing page relay command associated with the REFRESH button
        /// </summary>
        private RelayCommand landingPageRefreshRelayCommander;
        public ICommand LandingPageRefreshRelayCommander
        {
            get
            {
                if (null == landingPageRefreshRelayCommander)
                {
                    landingPageRefreshRelayCommander = new RelayCommand(ExecuteRefreshCommand);
                }
                return landingPageRefreshRelayCommander;
            }
        }

        /// <summary>
        /// Icon associated with the Stock Ticker.
        /// </summary>
        private string stockIcon;

        /// <summary>
        /// Icon associated with the Stock Ticker.
        /// </summary>
        public string StockIcon
        {
            get 
            { 
                return stockIcon; 
            }
            set
            {
                stockIcon = value;
                OnPropertyChanged("StockIcon");
            }
        }


        /// <summary>
        /// Timer which controls the TopStory Section
        /// </summary>
        private SlateTimer topStoryTimer;

        /// <summary>
        /// Timer which controls the Video Section
        /// </summary>
        private SlateTimer videoTimer;

        /// <summary>
        /// Timer which controls the Image Section
        /// </summary>
        private SlateTimer imageTimer;

        /// <summary>
        /// For Retrieving the Latest News Details
        /// </summary>
        private SlateTimer latestNewsTimer;

        /// <summary>
        /// Match Score with the associated number of overs.
        /// </summary>
        private string scoreSummary;

        /// <summary>
        /// Match score with the associated number of overs.
        /// </summary>
        public string ScoreSummary
        {
            get 
            { 
                return scoreSummary; 
            }
            set
            {
                scoreSummary = value;
                OnPropertyChanged("ScoreSummary");
            }
        }

        /// <summary>
        /// Batsman and Bowler Details for the ongoing match.
        /// </summary>
        private string scoreDetails;

        /// <summary>
        /// Batsman and Bowler Details for the ongoing match.
        /// </summary>
        public string ScoreDetails
        {
            get 
            { 
                return scoreDetails; 
            }
            set
            {
                scoreDetails = value;
                OnPropertyChanged("ScoreDetails");
            }
        }

        /// <summary>
        /// Temperature in Celsius.
        /// </summary>
        private string temperature;

        /// <summary>
        /// Temperature in Celsius.
        /// </summary>
        public string Temperature
        {
            get 
            { 
                return temperature; 
            }
            set
            {
                if (null != temperature && null != value && value.Equals(temperature))
                {
                    return;
                }
                temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        /// <summary>
        /// Name of the city for which the weather is being fetched.
        /// </summary>
        private string weatherCity;

        /// <summary>
        /// Name of the city for which the weather is being fetched.
        /// </summary>
        public string WeatherCity
        {
            get 
            { 
                return weatherCity; 
            }
            set
            {
                if (null != weatherCity && null != value && value.Equals(weatherCity))
                {
                    return;
                }
                weatherCity = value;
                OnPropertyChanged("WeatherCity");
            }
        }

        /// <summary>
        /// Current Status of the Match.
        /// </summary>
        private string matchStatus;

        /// <summary>
        /// Current Status of the Match.
        /// </summary>
        public string MatchStatus
        {
            get 
            { 
                return matchStatus; 
            }
            set
            {
                matchStatus = value;
                OnPropertyChanged("MatchStatus");
            }
        }

        /// <summary>
        /// Icon associated with the weather
        /// </summary>
        private string weatherIcon;

        /// <summary>
        /// Icon associated with the weather
        /// </summary>
        public string WeatherIcon
        {
            get 
            { 
                return weatherIcon; 
            }
            set
            {
                if (null != weatherIcon && null != value && value.Equals(weatherIcon))
                {
                    return;
                }
                weatherIcon = value;
                OnPropertyChanged("WeatherIcon");
            }
        }

        /// <summary>
        /// Timer to indicate when a request for the CricketShortScoreCard is to be initiated.
        /// </summary>
        private Timer masterTimer;

        /// <summary>
        /// Time required to Swap scores between Cricket Matches.
        /// </summary>
        private Timer swapDataTimer;

            
        /// <summary>
        /// List of recent Matches.
        /// </summary>
        private List<CricketFixtures> recentScorecards;

        /// <summary>
        /// List of live Matches.
        /// </summary>
        private List<CricketShortScorecard> liveScorecards;

        /// <summary>
        /// Live scorecards.
        /// </summary>
        public List<CricketShortScorecard> LiveScorecards
        {
            get 
            {
                return liveScorecards; 
            }
        }

        /// <summary>
        /// Bool value to indicate if there is a live match running
        /// </summary>
        private bool isMatchLive;

        /// <summary>
        /// Boolean value to indicate if there is a live match running
        /// </summary>
        public bool IsMatchLive
        {
            get
            {
                return isMatchLive;
            }
        }

        /// <summary>
        /// Details of the existing StockMarkets.
        /// </summary>
        public ObservableCollection<StockIndexItem> StockTickerDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Selected Cricket Score Card of a recent match.
        /// </summary>
        private int recentMatchSelectedScorecard;

        /// <summary>
        /// Selected Cricked Score card of a live match.
        /// </summary>
        private int liveMatchSelectedScorecard;

        /// <summary>
        /// Value after which the request is sent yet again.
        /// </summary>
        private int refetchDataDuration;

        /// <summary>
        /// Value after which if multiple values exist,swapping takes place.
        /// </summary>
        private int swapDataDuration;

        /// <summary>
        /// The selected Scorecard.
        /// </summary>
        public int RecentMatchSelectedScorecard
        {
            get 
            {
                return recentMatchSelectedScorecard; 
            }
        }

        /// <summary>
        /// Selected scorecard belonging to a Live Match
        /// </summary>
        public int LiveMatchSelectedScorecard
        {
            get
            {
                return liveMatchSelectedScorecard;
            }
        }

        /// <summary>
        /// List of Cricket Scorecards.
        /// </summary>
        public List<CricketFixtures> RecentScorecards
        {
            get 
            { 
                return recentScorecards; 
            }
        }

        //Flag which is used to check if the LandingPage is loaded for the first time, previous session details are made use of and relevant category is displayed on "FirstTime = True"
        private bool firstTime = true;

        //Internal flag used to indicate if Top Story details are loaded, the flag is set to 'TRUE' if the details are successfully retirevd or on error
        private bool isTopStoryLoaded;
        //Internal flag used to indicate if Video details are loaded, the flag is set to 'TRUE' if the details are successfully retirevd or on error
        private bool isVideoLoaded;
        //Internal flag used to indicate if Photo details are loaded, the flag is set to 'TRUE' if the details are successfully retirevd or on error
        private bool isPhotoLoaded;

        /// <summary>
        /// Default constructor 
        /// </summary>
        public LandingPageViewModel()
        {
            this.StockTickerDetails = new ObservableCollection<StockIndexItem>();
            this.refetchDataDuration = int.Parse(Utilities.Utility.GetTimerInterval("MasterTimerInterval"), CultureInfo.InvariantCulture);
            this.swapDataDuration = int.Parse(Utilities.Utility.GetTimerInterval("SwapTimerInterval"), CultureInfo.InvariantCulture);

            #region Timer Settings

            this.masterTimer = new Timer();
            this.swapDataTimer = new Timer();

            this.masterTimer.Interval = this.refetchDataDuration;
            this.swapDataTimer.Interval = this.swapDataDuration;
            this.masterTimer.Elapsed += OnMasterTimerElapsed;
            this.swapDataTimer.Elapsed += OnSwapDataTimerElapsed;

            topStoryTimer = new SlateTimer(int.Parse(Utilities.Utility.GetTimerInterval("TopStoryMainPage"), CultureInfo.InvariantCulture), TopStoriesRefresh);
            videoTimer = new SlateTimer(int.Parse(Utilities.Utility.GetTimerInterval("VideosMainPage"), CultureInfo.InvariantCulture), VideosRefresh);
            imageTimer = new SlateTimer(int.Parse(Utilities.Utility.GetTimerInterval("ImagesMainPage"), CultureInfo.InvariantCulture), ImagesRefresh);
            latestNewsTimer = new SlateTimer(int.Parse(Utilities.Utility.GetTimerInterval("LatestNewsMainPage"), CultureInfo.InvariantCulture), LatestNewsRefresh);
            #endregion

            #region Collection CHanged Event Handlers

            TopStoriesContainer1.CollectionChanged +=CollectionChanged;
            TopStoriesContainer2.CollectionChanged += CollectionChanged;

            #endregion

            #region Begin Processing

            ProcessRequest(new CategoriesFeedRequest(), LoadCategoriesFeedResponse);//categories request
            ProcessRequest(new LatestNewsFeedRequest(), LoadLatestNewsFeedResponse);//latest news(breaking news) request
            if (string.IsNullOrEmpty(ApplicationData.Settings.NewsCategory))//No need of checking the bool "first time" flag, sometimes due to async mode it gets set to false in the LoadCategoriesFeedResponse
            {
                ProcessRequest(new TopStoryFeedRequest(), LoadTopStoryFeedResponse, ProcessError, true);//News Articles request
                ProcessRequest(new RelatedVideosRequest(ApplicationData.BuildVideosLink(VideoCategories.Featured)), LoadVideosNewsFeedResponse, ProcessError, true);//Videos details Request
                ProcessRequest(new ImageAlbumRequest(ApplicationData.BuildImagesLink(ImageCategories.Featured)), LoadPhotosNewsFeedResponse, ProcessError, true);//Image Album details Request
            }
            ProcessRequest(new StockIndexRequest(), LoadStockMarketData);
            ProcessRequest(new CricketShortScorecardRequest(), LoadCricketShortScoreData);
            ProcessRequest(new WeatherDetailsRequest(), LoadWeatherData);
            masterTimer.Start();
            swapDataTimer.Start();

            #endregion
        }




        /// <summary>
        /// The top stories can be updated either by
        /// automatically on REFRESH or force updation
        /// </summary>
        /// <param name="item"></param>
        private void UpdateTopStories(CategoryItem item)
        {
            if (null != topStoryTimer)
            {
                topStoryTimer.Stop();//used to stop the timer by forcing it not raise the event as this is now personally refreshed..
            }
            ProcessRequest(new TopStoryFeedRequest(item), LoadTopStoryFeedResponse, ProcessError, true);
        }

        /// <summary>
        /// The Videos can be updated either by
        /// automatically on REFRESH or force updation
        /// </summary>
        /// <param name="item"></param>
        private void UpdateVideos(CategoryItem item)
        {
            if (null != videoTimer)
            {
                videoTimer.Stop();//used to stop the timer by forcing it not raise the event as this is now personally refreshed..
            }
            ProcessRequest(new RelatedVideosRequest(ApplicationData.NewsRelatedVideoLink(item)), LoadVideosNewsFeedResponse, ProcessError, true);
        }

        /// <summary>
        /// The Images can be updated either by
        /// automatically on REFRESH or force updation
        /// </summary>
        /// <param name="item"></param>
        private void UpdateImages(CategoryItem item)
        {
            if (null != imageTimer)
            {
                imageTimer.Stop();//used to stop the timer by forcing it not raise the event as this is now personally refreshed..
            }
            ProcessRequest(new ImageAlbumRequest(ApplicationData.NewsRelatedImageLink(item)), LoadPhotosNewsFeedResponse, ProcessError, true);
        }

        /// <summary>
        /// This is a call to retrieve the latest news and is not Category dependent
        /// </summary>
        private void UpdateLatestNews()
        {
            if (null != latestNewsTimer)
            {
                latestNewsTimer.Stop();//used to stop the timer by forcing it not raise the event as this is now personally refreshed..
            }
            ProcessRequest(new LatestNewsFeedRequest(), LoadLatestNewsFeedResponse);
        }

        /// <summary>
        /// A preloader notifier to the view
        /// </summary>
        private void PreloaderNotification()
        {
            if (isTopStoryLoaded && isVideoLoaded && isPhotoLoaded)
                IsLandingPageLoaded = true;
            //else IsLandingPageLoaded = false; //commented out to handle the error section where in there is a possibility of top stories and photos loading but not videos.
        }

        /// <summary>
        /// A method which says if the category item is the one returned by the NDTV Source or not
        /// </summary>
        /// <param name="categoryItem">Category Item</param>
        /// <returns>Bool based on whether it is a category item or not</returns>
        internal bool IsCategoryFromNDTVSource(object categoryItem)
        {
            bool isCategory = false;
            if (categoryItem.GetType() == typeof(CategoryItem))
            {
                if ((categoryItem as CategoryItem).IsNDTVSourceCategory)
                {
                    isCategory = true;
                }
            }
            return isCategory;
        }

        /// <summary>
        /// A method which returns the category item title of the newly added Catgeories, (like Videos and Photos which are added into the list)
        /// </summary>
        /// <param name="categoryItem">Category Item(only 2 types present as of now)</param>
        /// <returns>String based on what typeof category item or not</returns>
        internal string CategoryType(object categoryItem)
        {
            switch ((categoryItem as CategoryItem).Title)
            {
                case Constants.TopStories.NewCategories.Videos:
                    return "Videos";
                default:
                    return "Photos";
            }
        }



        /// <summary>
        /// This method refreshes the ticker data forcefully
        /// </summary>
        public void RefreshTickerData()
        {
            masterTimer.Stop();
            swapDataTimer.Stop();            
            ProcessRequest(new CricketShortScorecardRequest(), LoadCricketShortScoreData, null, false);
            ProcessRequest(new WeatherDetailsRequest(), LoadWeatherData, null, false);
            ProcessRequest(new StockIndexRequest(), LoadStockMarketData, null, false);
            masterTimer.Start();
            swapDataTimer.Start();
        }
    

        /// <summary>
        /// A method which is called from the view to reset the flags..
        /// This is the methos which switches the preloader on.
        /// It is the combination of these 3 flags which say whether the page is loaded or not(isTopStoryLoaded, isVideoLoaded, isPhotoLoaded)
        /// </summary>
        public void SwitchPreloaderOn()
        {
            isTopStoryLoaded = false;
            isVideoLoaded = false;
            isPhotoLoaded = false;
            IsLandingPageLoaded = false;
        }

        /// <summary>
        /// Disposes all the properties of view model 
        /// </summary>
        protected override void DisposeResources()
        {
            this.Category = null;
            this.masterTimer = null;
            this.imageTimer = null;
            this.LatestNews = null;
            this.latestNewsTimer = null;
            this.PhotoAlbums = null;
            this.ProfitIndices = null;
            this.recentScorecards = null;
            this.swapDataTimer = null;
            this.StockTickerDetails = null;
            this.TopStories = null;
            this.TopStoriesContainer1 = null;
            this.TopStoriesContainer2 = null;
            this.topStoryTimer = null;
            this.VideoItems = null;
            this.videoTimer = null;
            this.landingPageCategoryFlipRelayCommander = null;
            this.landingPageMenuItemRelayCommander = null;
            this.landingPageTopStoryItemsRelayCommander = null;
        }




        /// <summary>
        /// Method to load categories feed response
        /// </summary>
        /// <param name="response">Response object</param>
        private void LoadCategoriesFeedResponse(Response response)
        {
            Entities.Category responseObject = (response as CategoriesFeedResponse).Category;
            responseObject.AddCategory(Constants.TopStories.NewCategories.Videos);
            responseObject.AddCategory(Constants.TopStories.NewCategories.Photos);
            Category = responseObject;

            if (firstTime)
            {
                firstTime = false;
                if (!string.IsNullOrEmpty(ApplicationData.Settings.NewsCategory))
                {
                    ActiveSelectedCategoryItem = (from categoryItem in Category.CategoryItemList
                                                  where categoryItem.Title.ToUpperInvariant().Equals(ApplicationData.Settings.NewsCategory.ToUpperInvariant())
                                                  select categoryItem).First();

                    if (ActiveSelectedCategoryItem != null)
                    {
                        UpdateTopStories(ActiveSelectedCategoryItem);
                        UpdateImages(ActiveSelectedCategoryItem);
                        UpdateVideos(ActiveSelectedCategoryItem);
                    }
                }
            }


            #region CricketCategory Item set in the Application class

            if (null != Category && null != Category.CategoryItemList && Category.CategoryItemList.Count > 0)
            {
                List<CategoryItem> categoryItems = Category.CategoryItemList.Where(item => (item.Title == Constants.TopStories.Categories.Cricket)).ToList();
                if (null != categoryItems && categoryItems.Count > 0)
                {
                    ApplicationData.CricketCategoryItem = categoryItems[0];
                }
            }

            #endregion

            #region ProfitNewsCategoryItem set in Application class

            if (null != Category && null != Category.CategoryItemList && Category.CategoryItemList.Count > 0)
            {
                List<CategoryItem> categoryItems = Category.CategoryItemList.Where(item => (item.Title == Constants.TopStories.Categories.Business)).ToList();
                if (null != categoryItems && categoryItems.Count > 0)
                {
                    ApplicationData.BusinessCategoryItem = categoryItems[0];
                }
            }

            #endregion
        }

        /// <summary>
        /// Method to load top story feed response
        /// </summary>
        /// <param name="response">Response object</param>
        private void LoadTopStoryFeedResponse(Response response)
        {
            TopStories = ((TopStoryFeedResponse)response).TopStory;

            TopStoriesContainer1 = new ObservableCollection<TopStoryItem>();
            TopStoriesContainer2 = new ObservableCollection<TopStoryItem>();

            DataFragmentation<TopStoryItem>.Fragment(2, TopStories.TopStoryCollection,
               new List<ObservableCollection<TopStoryItem>>() { TopStoriesContainer1, TopStoriesContainer2 },
               DataFragmentation<TopStoryItem>.FragmentedDataflow.RightToLeft,
               Utility.GetNumberOfItemsToRetrieve("TopStoriesLandingPage"));
            TopStoriesContainer3 = TopStories.RetrieveFirstArticle();
            if (null != TopStoriesContainer3)
            {
                TopStoriesContainer4 = new ObservableCollection<TopStoryItem>();
                DataFragmentation<TopStoryItem>.Fragment(1,
                    TopStories.TopStoryCollection.Except<TopStoryItem>(new List<TopStoryItem>() { TopStoriesContainer3 }).ToList(),
                   new List<ObservableCollection<TopStoryItem>>() { TopStoriesContainer4 },
                   DataFragmentation<TopStoryItem>.FragmentedDataflow.UpToDown,
                   Utility.GetNumberOfItemsToRetrieve("TopStoriesLandingPage") - 1);
            }

            isTopStoryLoaded = true;
            PreloaderNotification();
            topStoryTimer.Start();
        }

        /// <summary>
        /// Method to load latest news feed response
        /// </summary>
        /// <param name="response">Response Object</param>
        private void LoadLatestNewsFeedResponse(Response response)
        {
            LatestNews = ((LatestNewsFeedResponse)response).LatestNewsDetails;
            latestNewsTimer.Start();
        }

        /// <summary>
        /// This is the method called after videos loading..
        /// </summary>
        /// <param name="response">Response Object</param>
        private void LoadVideosNewsFeedResponse(Response response)
        {
            VideoItems = (ObservableCollection<VideoItem>)((RelatedVideosResponse)response).RelatedVideoList;
            isVideoLoaded = true;
            PreloaderNotification();
            videoTimer.Start();
        }

        /// <summary>
        /// Call back method after loading the image albums
        /// </summary>
        /// <param name="response">Response Object</param>
        private void LoadPhotosNewsFeedResponse(Response response)
        {
            if (null != response && (response.GetType().Equals(typeof(ImageAlbumResponse))))
            {
                PhotoAlbums = ((ImageAlbumResponse)response).ImageAlbumCollection;
            }
            isPhotoLoaded = true;
            PreloaderNotification();
            imageTimer.Start();
        }

        /// <summary>
        /// Master Timer to indicate when the data has to be re-fetched.Request sent again.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void OnMasterTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ProcessRequest(new CricketShortScorecardRequest(), LoadCricketShortScoreData,null,false);
            ProcessRequest(new WeatherDetailsRequest(), LoadWeatherData,null,false);
            ProcessRequest(new StockIndexRequest(), LoadStockMarketData,null,false);
        }

        /// <summary>
        /// Loads the Stock market related data for the ticker to display.
        /// </summary>
        /// <param name="response">Response associated with the Request call</param>
        private void LoadStockMarketData(Response response)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                if (null != response)
                {
                    var responseCast = (response as StockIndexResponse);
                    if (null != responseCast)
                    {
                        if (null != this.StockTickerDetails)
                        {
                            if (this.StockTickerDetails.Count != 0)
                            {
                                this.StockTickerDetails.Clear();
                            }
                        }
                        else
                        {
                            this.StockTickerDetails = new ObservableCollection<StockIndexItem>();
                        }
                        if (responseCast.StockIndexList.Count > 0)
                        {
                            foreach (var stock in responseCast.StockIndexList)
                            {
                                this.StockTickerDetails.Add(stock);
                            }
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// Loads the Cricket Short Score Card related data for the Ticker to display.
        /// </summary>
        /// <param name="response">Response associated with the Request call</param>
        private void LoadCricketShortScoreData(Response response)
        {
            this.isMatchLive = false;
            if (null != response)
            {
                this.liveScorecards = null;
                this.liveScorecards = new List<CricketShortScorecard>((response as CricketShortScorecardResponse).Scorecards);

                if (null != this.liveScorecards && this.liveScorecards.Count > 0)
                {
                    foreach (var item in this.liveScorecards)
                    {
                        int value;
                        if (int.TryParse(item.IsLive, out value))
                        {
                            if (value == 1)
                            {
                                this.isMatchLive = true;
                            }
                        }
                    }
                }

                if (true == this.isMatchLive)
                {
                    if (null != this.liveScorecards && this.liveScorecards.Count > 0)
                    {
                        //Assign Live Data
                        this.AssignLiveCricketMatchData();
                    }
                }
                else
                {
                    ProcessRequest(new CricketFixturesRequest(), LoadRecentMatchesList);
                }
            }
        }

        /// <summary>
        /// Loads Recent Matches List
        /// </summary>
        /// <param name="response">Appropriate response</param>
        private void LoadRecentMatchesList(Response response)
        {
            if (null != response)
            {
                this.recentScorecards = null;
                this.recentScorecards = new List<CricketFixtures>((response as CricketFixturesResponse).RecentMatchList);
                if (null != this.recentScorecards && this.recentScorecards.Count>0)
                {
                    this.AssignRecentCricketMatchesData();
                }
            }
        }

        /// <summary>
        /// Loads the Weather Related Data for the Ticker to display.
        /// </summary>
        /// <param name="response">Response associated with the Request call</param>
        private void LoadWeatherData(Response response)
        {
            if (null != response)
            {
                WeatherData weatherItem = (response as WeatherDetailsResponse).CityWeatherData;
                if (null != weatherItem)
                {
                    this.Temperature = string.Format(CultureInfo.InvariantCulture, Resources.CelsiusSuffix, weatherItem.TemperatureCelsius);
                    this.WeatherCity = weatherItem.CityName;
                    this.WeatherIcon = weatherItem.Image;
                }
                else
                {
                    this.Temperature = "NA";
                    this.WeatherCity = ApplicationData.Settings.WeatherCity;
                    this.WeatherIcon = Path.Combine(Utility.FetchImagePath(),Constants.Constant.DefaultWeatherIcon);
                }
            }
        }

        /// <summary>
        /// Event handler to indicate the end of the SwapData time. Handler moves between multiple data across the same list.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        private void OnSwapDataTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (true == this.isMatchLive)
            {
                if (null != this.liveScorecards)
                {
                    if (liveMatchSelectedScorecard == this.liveScorecards.Count - 1)
                    {
                        liveMatchSelectedScorecard = 0;
                    }
                    else
                    {
                        liveMatchSelectedScorecard++;
                    }
                }
                this.AssignLiveCricketMatchData();
            }
            else
            {
                if (null != this.recentScorecards)
                {
                    if (recentMatchSelectedScorecard == this.recentScorecards.Count - 1)
                    {
                        recentMatchSelectedScorecard = 0;
                    }
                    else
                    {
                        recentMatchSelectedScorecard++;
                    }
                }
                this.AssignRecentCricketMatchesData();
            }
        }

        /// <summary>
        /// Associate data with the properties bound to the User Interface.
        /// </summary>
        private void AssignRecentCricketMatchesData()
        {
            if (null != this.recentScorecards && this.recentScorecards.Count > 0 && this.recentMatchSelectedScorecard < this.recentScorecards.Count && this.recentMatchSelectedScorecard >= 0)
            {
                if (true == this.recentScorecards[recentMatchSelectedScorecard].Recent)
                {
                    this.MatchStatus = string.Concat("RECENT:", " ");
                    this.ScoreSummary = string.Concat(this.recentScorecards[recentMatchSelectedScorecard].TeamAShortName, " vs ", this.recentScorecards[recentMatchSelectedScorecard].TeamBShortName);
                    this.ScoreDetails = string.Concat(this.recentScorecards[recentMatchSelectedScorecard].MatchResult);
                }
            }
        }

        private void AssignLiveCricketMatchData()
        {
            if (null != this.liveScorecards && this.liveScorecards.Count > 0 && this.liveMatchSelectedScorecard < this.liveScorecards.Count && this.liveMatchSelectedScorecard >= 0)
            {
                int value;
                if (int.TryParse(this.liveScorecards[liveMatchSelectedScorecard].IsLive,out value))
                {
                    if (value == 1)
                    {
                        this.ScoreSummary = string.Format(CultureInfo.InvariantCulture, Resources.CricketScoreSummary, this.liveScorecards[liveMatchSelectedScorecard].ShortenedTeamName, this.liveScorecards[liveMatchSelectedScorecard].CurrentScore, this.liveScorecards[liveMatchSelectedScorecard].CurrentOver);
                        this.ScoreDetails = string.Concat(this.liveScorecards[liveMatchSelectedScorecard].MatchType, Constants.Constant.AtText, this.liveScorecards[liveMatchSelectedScorecard].Venue.Substring(this.liveScorecards[liveMatchSelectedScorecard].Venue.IndexOf(',') + 2));
                        this.MatchStatus = string.Concat(Resources.LiveText, " ");
                    }
                }
            }
        }
        
        /// <summary>
        /// Handler which is used to handle all the errors
        /// On error stop the Preloader first and then Reset all the preloader flags..
        /// The flag would be true despite a few Process requests breaking..
        /// </summary>
        /// <param name="req">Request sent back from the connector incase if any error while GET/POST</param>
        private void ProcessError(Request request)
        {
            IsLandingPageLoaded = true;
        }

        /// <summary>
        /// The timers call back function to refresh TopStories
        /// </summary>
        private void TopStoriesRefresh()
        {
            UpdateTopStories(ActiveSelectedCategoryItem);
        }

        /// <summary>
        /// The timers call back function to refresh Videos
        /// </summary>
        private void VideosRefresh()
        {
            UpdateVideos(ActiveSelectedCategoryItem);
        }

        /// <summary>
        /// The timers call back function to refresh Images
        /// </summary>
        private void ImagesRefresh()
        {
            UpdateImages(ActiveSelectedCategoryItem);
        }

        /// <summary>
        /// A method to refresh the Latest News
        /// </summary>
        private void LatestNewsRefresh()
        {
            UpdateLatestNews();
        }

        /// <summary>
        /// Method which will be called by the UI or the view.cs which does the actual implementation om click of a Category.
        /// </summary>
        /// <param name="parameter">Command parameter</param>
        private void ExecuteLandingPageMenuItemCommand(object parameter)
        {
            CategoryItem item = (CategoryItem)((MenuItem)parameter).DataContext;
            if (true == item.IsNDTVSourceCategory)
            {
                ActiveSelectedCategoryItem = item;
                UpdateTopStories(item);
                UpdateImages(item);
                UpdateVideos(item);
            }
            else
            {
                //set every preloader flag to true for the preloader..
                isTopStoryLoaded = true;
                isVideoLoaded = true;
                isPhotoLoaded = true;
                IsLandingPageLoaded = true;
            }
        }

        /// <summary>
        /// The command which is directly binded to the button
        /// </summary>
        private void ExecuteRefreshCommand(object parameter)
        {
            TopStoriesRefresh();
            VideosRefresh();
            ImagesRefresh();
            LatestNewsRefresh();
        }

        /// <summary>
        /// The method which gets called when the collection changes to help us the UI reflect this changes..
        /// </summary>
        /// <param name="sender">source containers</param>
        /// <param name="e">collection changed argument which is raised when the scroll bar reaches to the end in the user interface</param>
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(sender, e);
        }

        /// <summary>
        /// Method which is responsible for sharing articles among the available Containers
        /// </summary>
        /// <param name="parameter">relay command</param>
        private void CollectionHandler(object parameter)
        {
            DataFragmentation<TopStoryItem>.Fragment(2, TopStories.TopStoryCollection,
                new List<ObservableCollection<TopStoryItem>>() { TopStoriesContainer1, TopStoriesContainer2 },
                DataFragmentation<TopStoryItem>.FragmentedDataflow.RightToLeft,
                Utility.GetNumberOfItemsToRetrieve("TopStoriesLandingPage"));

            DataFragmentation<TopStoryItem>.Fragment(1,
                TopStories.TopStoryCollection.Except<TopStoryItem>(new List<TopStoryItem>() { TopStoriesContainer3 }).ToList(),
                  new List<ObservableCollection<TopStoryItem>>() { TopStoriesContainer4 },
                  DataFragmentation<TopStoryItem>.FragmentedDataflow.UpToDown,
                  Utility.GetNumberOfItemsToRetrieve("TopStoriesLandingPage"));

        }

        /// <summary>
        /// The module which Flips between Categories..
        /// </summary>
        /// <param name="parameter">parameter is used to track if the flipping is from LeftToRight or the other way..</param>
        private void CategoryFlipUnit(object parameter)
        {
            CategoryItem item = Category.CategoryItemList.Where(listItem => (listItem.Priority == (ActiveSelectedCategoryItem.Priority + int.Parse(parameter.ToString(),CultureInfo.CurrentCulture)))).First();
            if (null == item)
            {
                return;
            }
            ActiveSelectedCategoryItem = item;
            UpdateTopStories(item);
            UpdateImages(item);
            UpdateVideos(item);
        }
    }

    /// <summary>
    /// An internal Timer class
    /// </summary>
    public sealed class SlateTimer : Timer
    {
        #region Properties

        private int timeInterval;
        public delegate void InvokeMethod();
        InvokeMethod CallbackMethod;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor which instantiates the timer resources
        /// </summary>
        /// <param name="timeInterval">the timeinterval scheduled</param>
        /// <param name="callbackMethod">the callback method</param>
        public SlateTimer(int timeInterval, InvokeMethod callbackMethod)
        {
            this.timeInterval = timeInterval;
            CallbackMethod = callbackMethod;

            Interval = timeInterval;
            Elapsed += new ElapsedEventHandler(TimerCallbackMethod);
        }

        /// <summary>
        /// The event that gets invoked by the timer
        /// </summary>
        /// <param name="sender">the timer is the sender in this case</param>
        /// <param name="e">arguments associated to the broke event</param>
        void TimerCallbackMethod(object sender, ElapsedEventArgs e)
        {
            CallbackMethod();
        }

        #endregion
    }
}