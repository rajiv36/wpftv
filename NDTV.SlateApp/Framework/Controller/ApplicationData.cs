using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using NDTV.Entities;
using NDTV.Interfaces;
using NDTV.Utilities;
using System;

namespace NDTV.Controller
{
    /// <summary>
    /// This is a static container which contains objects used throughout the application
    /// </summary>
    public static class ApplicationData
    {
        public static double DpiX { get; set; }
        /// <summary>
        /// Text about NDTV.
        /// </summary>
        public static string AboutNdtvText { get; set; }

        /// <summary>
        /// Current item updated event handler.
        /// </summary>
        public static event EventHandler CurrenItemUpdated;
        
        /// <summary>
        /// Pop up open value changed handler.
        /// </summary>
        public static event EventHandler IsPopUpOpenValueChanged;

        /// <summary>
        /// List of Indian cities.
        /// </summary>
        public static List<string> IndianCities
        {
            get;
            set;
        }

        /// <summary>
        /// List of Foreign cities.
        /// </summary>
        public static List<string> ForeignCities
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the total number of images in the selected Album.
        /// </summary>
        public static ObservableCollection<int> TotalImagesOfSelectedAlbum
        {
            get;
            set;
        }

        private static NDTVController controller;        

        /// <summary>
        /// Gets controller instance
        /// </summary>
        public static NDTVController Controller
        {
            get
            {
                if (null == controller)
                {
                    controller = new NDTVController();
                }
                return controller;
            }
        }

        /// <summary>
        /// Indicates the social media via which a login
        /// was carried out first.
        /// </summary>
        private static LoginMode firstLoggedOn;
        public static LoginMode FirstLoggedOn
        {
            get
            {
                return firstLoggedOn;
            }
            set
            {
                firstLoggedOn = value;
                if (null != UserInstance)
                {
                    UpdateUserInstance(value);
                }
            }

        }

        private static void UpdateUserInstance(LoginMode loginMode)
        {
            UserInstance.LoginMode = loginMode;
            switch (loginMode)
            {
                case LoginMode.Facebook:
                    FacebookAccount fbAccount = ApplicationData.FacebookAccount as FacebookAccount;
                    ApplicationData.UserInstance.ProfileLink = fbAccount.FacebookUser.PhotoLink;
                    ApplicationData.UserInstance.LoginMode = LoginMode.Facebook;
                    ApplicationData.UserInstance.DisplayName = fbAccount.FacebookUser.Name;
                    break;

                case LoginMode.Twitter:
                    TwitterAccount twitterAccount = ApplicationData.TwitterAccount as TwitterAccount;
                    ApplicationData.UserInstance.ProfileLink = twitterAccount.TwitterUser.PhotoLink;
                    ApplicationData.UserInstance.LoginMode = LoginMode.Twitter;
                    ApplicationData.UserInstance.DisplayName = twitterAccount.TwitterUser.Name;
                    break;

                case LoginMode.Google:
                    GoogleAccount googleAccount = ApplicationData.GoogleAccount as GoogleAccount;
                    ApplicationData.UserInstance.ProfileLink = googleAccount.GoogleUser.Details.PhotoLink;
                    ApplicationData.UserInstance.LoginMode = LoginMode.Google;
                    ApplicationData.UserInstance.DisplayName = googleAccount.GoogleUser.Details.Name.Short;
                    break;

                case LoginMode.Yahoo:
                    YahooAccount yahooAccount = ApplicationData.YahooAccount as YahooAccount;
                    ApplicationData.UserInstance.ProfileLink = yahooAccount.YahooUser.Details.ImageDetails.PhotoLink;
                    ApplicationData.UserInstance.LoginMode = LoginMode.Yahoo;
                    ApplicationData.UserInstance.DisplayName = yahooAccount.YahooUser.Details.Name;
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the user ID of the logged in user.
        /// </summary>
        public static UserData UserInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current item.
        /// </summary>
        public static ShareData CurrentItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the facebook account.
        /// </summary>
        public static Account FacebookAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the twitter account.
        /// </summary>
        public static Account TwitterAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the twitter account.
        /// </summary>
        public static Account LinkedInAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the google account.
        /// </summary>
        public static Account GoogleAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the yahoo account.
        /// </summary>
        public static Account YahooAccount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating if the 
        /// pop up is open.
        /// </summary>
        private static bool isPopUpOpen;
        public static bool IsPopUpOpen
        {
            get
            {
                return isPopUpOpen;
            }
            set
            {
                isPopUpOpen = value;
                if (null != IsPopUpOpenValueChanged)
                {
                    IsPopUpOpenValueChanged(null, null);
                }
            }
        }

        private static ObservableCollection<VideoListCategories> relatedVideoList;

        /// <summary>
        /// Collection which holds the related videos for all categories
        /// </summary>
        public static ObservableCollection<VideoListCategories> RelatedVideoList
        {
            get { return relatedVideoList; }
            set { relatedVideoList = value; }
        }        

        /// <summary>
        /// Collection which holds the related images categories.
        /// </summary>
        public static List<ImageCategoryItem> ImagesCategoryList
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static CategoryItem CricketCategoryItem
        {
            get;
            set;
        }

        /// <summary>
        /// Business category Item,
        /// </summary>
        public static CategoryItem BusinessCategoryItem
        {
            get;
            set;
        }

        /// <summary>
        /// Collection to hold the Metadata assoicated with the Stock Market.
        /// </summary>
        public static ObservableCollection<StockMarketData> StockMarketList
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the current item to be shared.
        /// </summary>
        /// <param name="title">Share item title</param>
        /// <param name="description">Share item description</param>
        /// <param name="guid">Share item  guid</param>
        /// <param name="imageLink">Share item image link</param>
        /// <param name="link">Share item link</param>
        /// <param name="shareType">Share item type</param>
        public static void SetCurrentItem(string title, string description, string guid, string imageLink, string link, ShareMediaType shareType)
        {
            ApplicationData.CurrentItem = new ShareData();
            ApplicationData.CurrentItem.Caption = title;
            ApplicationData.CurrentItem.Description = description;
            ApplicationData.CurrentItem.Guid = guid;
            ApplicationData.CurrentItem.ImageLink = imageLink;
            ApplicationData.CurrentItem.Link = link;
            ApplicationData.CurrentItem.ShareType = shareType;

            if (null != CurrenItemUpdated)
            {
                CurrenItemUpdated(null, null);
            }
        }

        /// <summary>
        /// Filter the list based on the input category.
        /// </summary>
        /// <returns>return the url for related videos</returns>
        public static string BuildVideosLink(VideoCategories category)
        {
            if (RelatedVideoList.Count > 0)
            {
                var videoCategory = from item in RelatedVideoList
                                    where item.CategoryTitle.Replace(" ","").ToUpperInvariant().Equals(
                                    category.ToString().ToUpperInvariant())
                                    select item.Link;
                return videoCategory.First().ToString(CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        private static ILogger errorLogger;

        /// <summary>
        /// Gets the error logger
        /// </summary>
        public static ILogger ErrorLogger
        {
            get
            {
                if (null == errorLogger)
                {
                    errorLogger = new Logger();
                }
                return errorLogger;
            }
        }

        /// <summary>
        /// Filter the list based on the input category.
        /// </summary>
        /// <returns>return the url for related album</returns>
        public static string BuildImagesLink(ImageCategories category)
        {
            var imageCategory = from item in ImagesCategoryList
                                where item.ImageAlbumTitle.ToUpperInvariant().Equals(
                                category.ToString().ToUpperInvariant())
                                select item.ImageAlbumLink;
            return imageCategory.First().ToString();
        }

        /// <summary>
        /// The mapping method which returns back a video category for a particular category item..
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static string NewsRelatedVideoLink(CategoryItem cat)
        {
            VideoCategories videoCategory;
            switch (cat.Title)
            {
                case Constants.TopStories.Categories.Business:
                    videoCategory = VideoCategories.Business;break;
                case Constants.TopStories.Categories.Cities:
                    videoCategory = VideoCategories.News;break;
                case Constants.TopStories.Categories.Cricket:
                    videoCategory = VideoCategories.Sports;break;
                case Constants.TopStories.Categories.Entertainment:
                    videoCategory = VideoCategories.Entertainment;break;
                case Constants.TopStories.Categories.India:
                    videoCategory = VideoCategories.News;break;
                case Constants.TopStories.Categories.Recent:
                    videoCategory = VideoCategories.Featured;break;
                case Constants.TopStories.Categories.Sports:
                    videoCategory = VideoCategories.Sports;break;
                case Constants.TopStories.Categories.Technology:
                    videoCategory = VideoCategories.Business;break;
                case Constants.TopStories.Categories.TopStory:
                    videoCategory = VideoCategories.Featured;break;
                case Constants.TopStories.Categories.Trending:
                    videoCategory = VideoCategories.Exclusive;break;
                case Constants.TopStories.Categories.World:
                    videoCategory = VideoCategories.News ;break;
                default:
                    videoCategory = VideoCategories.Business;break;
            }
            return BuildVideosLink(videoCategory);
        }

        /// <summary>
        /// The mapping method which returns back a image category for a particular category item..
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public static string NewsRelatedImageLink(CategoryItem cat)
        {
            ImageCategories imageCategory;
            switch (cat.Title)
            {
                case Constants.TopStories.Categories.Business:
                    imageCategory = ImageCategories.Business; break;
                case Constants.TopStories.Categories.Cities:
                    imageCategory = ImageCategories.News; break;
                case Constants.TopStories.Categories.Cricket:
                    imageCategory = ImageCategories.Sports; break;
                case Constants.TopStories.Categories.Entertainment:
                    imageCategory = ImageCategories.Entertainment; break;
                case Constants.TopStories.Categories.India:
                    imageCategory = ImageCategories.News; break;
                case Constants.TopStories.Categories.Recent:
                    imageCategory = ImageCategories.Featured; break;
                case Constants.TopStories.Categories.Sports:
                    imageCategory = ImageCategories.Sports; break;
                case Constants.TopStories.Categories.Technology:
                    imageCategory = ImageCategories.Gadgets; break;
                case Constants.TopStories.Categories.TopStory:
                    imageCategory = ImageCategories.Featured; break;
                case Constants.TopStories.Categories.Trending:
                    imageCategory = ImageCategories.Latest; break;
                case Constants.TopStories.Categories.World:
                    imageCategory = ImageCategories.News; break;
                default:
                    imageCategory = ImageCategories.Business; break;
            }
            return BuildImagesLink(imageCategory);
        }

        /// <summary> 
        /// Gets or sets the response for cricket match fixtures 
        /// </summary> 
        public static CricketFixturesResponse MatchFixtureResponse
        { 
            get;
            set;            
        }

        /// <summary>
        /// Gets or sets whether the application is in landscape orientation. If false, it means the application is in portrait orientation
        /// </summary>
        public static bool IsLandscapeOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the application is online or not
        /// </summary>
        public static bool IsApplicationOnline
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieves the cricket fixture entity based on the provided fileName.
        /// </summary>
        /// <param name="fileName">fileName - filename for xml from which scorecard is to be extracted</param>
        /// <param name="fixture">CricketFixture - Live, upcoming or recent</param>
        /// <returns>CricketFixtures entity</returns>
        public static CricketFixtures RetrieveCricketFixture(string fileName, CricketFixture fixture)
        {
            if (null != fileName)
            {
                try
                {
                    switch (fixture)
                    {
                        case CricketFixture.Live:
                            if (MatchFixtureResponse.LiveMatchList.Count > 0 && MatchFixtureResponse.LiveMatchList.Any(matchFile => matchFile.MatchFile.Equals(fileName)))
                            {
                                return MatchFixtureResponse.LiveMatchList.Where(file => file.MatchFile.Equals(fileName)).First();
                            }
                            else
                            {
                                return null;
                            }
                        case CricketFixture.Recent:
                            if (MatchFixtureResponse.RecentMatchList.Count > 0 && MatchFixtureResponse.RecentMatchList.Any(matchFile => matchFile.MatchFile.Equals(fileName)))
                            {
                                return MatchFixtureResponse.RecentMatchList.Where(file => file.MatchFile.Equals(fileName)).First();
                            }
                            else
                            {
                                return null;
                            }
                        case CricketFixture.Upcoming:
                            if (MatchFixtureResponse.UpcomingMatchList.Count > 0 && MatchFixtureResponse.UpcomingMatchList.Any(matchFile => matchFile.MatchFile.Equals(fileName)))
                            {
                                return MatchFixtureResponse.UpcomingMatchList.Where(file => file.MatchFile.Equals(fileName)).First();
                            }
                            else
                            {
                                return null;
                            }
                    }
                    return null;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the flag which denotes whether the error message popup is visible
        /// </summary>
        public static bool IsErrorMessagePopupVisible
        {
            get;
            set;
        }

        private static ApplicationSettings settings;

        /// <summary>
        /// Gets the application settings
        /// </summary>
        public static ApplicationSettings Settings
        {
            get
            {
                if (null == settings)
                {
                    settings = Utility.LoadSettings();
                }

                return settings;
            }
        }
    }
}
