using NDTV.Utilities;
namespace NDTV.Entities
{
    /// <summary>
    /// Contains all the constants used in the entities
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Class holding static text constants
        /// </summary>
        public static class StaticText
        {
            public const string AboutNdtvDefaultText = "NDTV's tryst with television began way back in 1988, when it produced a news and current affairs show 'The World This Week' for the Government-owned broadcaster Doordarshan. The show proved to be immensely popular, and NDTV established its image as a credible private news producer. It moved on to become the sole news content provider and producer for India's first 24-hour news channel, Star News. The company's biggest milestone was established in 2003, when it launched two 24-hour news channelsâ€” NDTV 24x7 in English and NDTV India in Hindiâ€”followed by a 24-hour business news channel NDTV Profit in 2005, which became a leading business news channel in a short period of time." +
                "In 2007, the company broke new ground, formulating astrategy to build a global business. NDTV Networks Plc, a subsidiary of NDTV was formed to seize opportunities in areas 'beyond news'. The Company launched NDTV Convergence to exploit the synergies between television, internet and mobile. NDTV mobile offerings include the WAP portal, m.ndtv.com, a one-stop shop for news and infotainment content on the go. It is the leading mobile content partner for telecom operators in India, and has partnered with leading handset manufacturers, including the iPhone, for premium content offerings. The NDTV digital portfolio also includes leading web properties, including, but not limited to, www.ndtv.com.";

            public const string AboutAditiText = "Founded in 1994, Aditi pioneered the idea that great software products can be built out of India. Today, Aditi represents the best Microsoft software & applications development capability in the world. Strengthened by innovative IP solutions, leading global enterprises and software product companies rely on Aditi to predictably deliver quality products and applications." +
                                                  " www.Aditi.com";

            public const string AboutApplicationText = "NDTV Slate ver 1.0";

            public const string TermsAndConsitions = "I Don't know...";

        }
    

        /// <summary>
        /// Class holding the URL keys constants.
        /// </summary>
        public static class LinkNames
        {
            public const string YearHighLink = "52WeekHighStocks";
            public const string YearLowLink = "52WeekLowStocks";
            public const string SensexStocksLink = "SensexStocks";
            public const string NiftyStocksLink = "NiftyStocks";
            public const string CategoriesFeedLink = "CategoriesFeed";
            public const string CompanySpecificDataLink = "CompanySpecificData";
            public const string TopStoryFeedLink = "TopStoryFeed";
            public const string FlashNewsfeedLink = "FlashNewsFeed";
            public const string VideosFeedLink = "VideosFeed";
            public const string PhotosFeedLink = "PhotosFeed";
            public const string ProfitIndexFeedLink = "ProfitIndexFeed";
            public const string StockFeedLink = "StockExchangeFeed";
            public const string StockIndexesLink = "StockIndicesFeed";
            public const string CricketShortScorecardLink = "CricketShortScoreCardFeed";
            public const string WeatherDetailsLink = "WeatherFeed";
            public const string TVScheduleLink = "TVSchedule";
            public const string CricketScheduleLink = "CricketSchedule";
            public const string ImageResizerLink = "ImageResizer";
            public const string LatestNewsLink = "LatestNews";
            public const string ImageCategoriesLink = "ImageCategories";
            public const string ImageCategoriesAlbumLink = "ImageCategoriesAlbum";
            public const string CricketFullScorecard = "CricketFullScore";
            public const string CricketFixtures = "CricketFixtures";
            public const string CricketCommentary = "CricketCommentary";
            public const string ImageItemZoomDropLink = "ImageItemZoomDrop";
            public const string ImageItemZoomReplaceLink = "ImageItemZoomReplace";
            public const string LiveTVNDTV24x7VideoLink = "NDTV24x7Video";
            public const string LiveTVNDTVIndiaVideoLink = "NDTVIndiaVideo";
            public const string LiveTVNDTVProfitVideoLink = "NDTVProfitVideo";
            public const string LiveTVNDTVGoodTimesVideoLink = "NDTVGoodtimesVideo";
            public const string LiveTVNDTV24x7Schedule = "NDTV24X7SCHEDULE";
            public const string LiveTVNDTVIndiaSchedule = "NDTVIndiaSCHEDULE";
            public const string LiveTVNDTVProfitSchedule = "NDTVProfitSCHEDULE";
            public const string LiveTVNDTVGoodTimesSchedule = "NDTVGoodtimesSCHEDULE";
            public const string LiveTVNDTVHinduSchedule = "NDTVHinduSCHEDULE";
            public const string LiveTV24x7Id = "LIVE_BG24x7";
            public const string LiveTVIndiaId = "LIVE_BGINDIA";
            public const string LiveTVProfitId = "LIVE_BGPROFIT";
            public const string LiveTVGoodTimesId = "LIVE_BGGT";
            public const string LiveTVHinduId = "LIVE_BGHINDU";
            public const string LiveTVVideoPlayerLink = "NDTVVideoPlayerURL";
            public const string NDTVWeatherPageLink = "NDTVWeatherPage";
            public const string ProfitNewsflashLink = "ProfitNewsFlash";
            public const string MailShareLink = "MailSharing";
            public const string MostActiveStocksLink = "MostActiveStocks";
            public const string MarketTopLosersLink = "MarketTopLosers";
            public const string MarketTopGainersLink = "MarketTopGainers";
            public const string MarketFullDashboardLink = "MarketFullDashboard";
            public const string VideoSearchLink = "VideoSearchLink";
            public const string PhotoSearchLink = "PhotoSearchLink";
            public const string NewsSearchLink = "NewsSearchLink";
            public const string AboutNDTVLink = "AboutNDTVLink";
        }

        public static class ReportErrorKeys
        {
            public const string FromEmailAddress = "FromAddress";
            public const string ToEmailAddress = "ToAddress";
        }

        /// <summary>
        /// Class holding the http method names.
        /// </summary>
        public static class HttpMethodNames
        {
            public const string Get = "GET";
            public const string Post = "POST";
        }

        /// <summary>
        /// Class holding general constants.
        /// </summary>
        public static class Constant
        {
            public const string XmlExtension = ".xml";
            public const string JpgExtension = ".jpg";
            public const string NewHighResolutionJpgExtension = "_180x135.jpg";
            public const string PngExtension = ".png";
            public const string GifExtension = ".gif";
            public const string TextExtension = ".txt";
            public const string OpeningBrace = "(";
            public const string ClosingBrace = ")";
            public const string Colon = ":";
            public const string Underscore = "_";
            public const string Hyphen = "-";
            public const string ForwardSlash = "/";
            public const string Commentary = "commentary";
            public const string NotOut = "not out";
            public const string LastTwelveBalls = "lasttwelve";
            public const string FullInnings = "fullinnings";
            public const string Resources = "Resources";
            public const string Images = "Images";
            public const string DefaultImage = "NDTVDefaultImage";
            public const string DefaultImageForRelatedNews = "DefaultImageForRelatedNews";
            public const string TestInnings = "Test";
            public const string FirstInn = "1st Inn";
            public const string SecondInn = "2nd Inn";
            public const string Vs = " vs ";
            public const string CommaWithSpace = ", ";
            public const string Space = " ";
            public const string FilePathSeparator = "\\";
            public const string DateFormat = "MMM dd, yyyy";
            public const string Yes = "Yes";
            public const string Asterisk = "*";
            public const string Day = "DAY";
            public const string StrikeRateFormat = "{0:0.##}";
            public const string FirstInnings = "FirstInnings";
            public const string SecondInnings = "SecondInnings";
            public const string ThirdInnings = "ThirdInnings";
            public const string FourthInnings = "FourthInnings";
            public const string Live = "LIVE";
            public const string Recent = "RECENT";
            public const string Upcoming = "UPCOMING";
            public const string Partnership = "Partnership ";
            public const string LastWicket = "Last Wkt ";
            public const string Batting = "Batting";
            public const string CommentaryLiteral = "&commentary=";
            public const string Tilde = "~";
            public const string PipeSeparator = "|";
            public const string FullStop = ".";
            public const string MostActiveStocks = "Most Active";
            public const string Gainers = "Gainers";
            public const string Losers = "Losers";
            public const string YearHigh = "52-Week High";
            public const string YearLow = "52-Week Low";
            public const string Sensex = "Sensex Stocks";
            public const string Nifty = "Nifty Stocks";
            public const string FullDashboard = "Full Dashboard";
            public const string AtText = " at ";
            public const string NotAvailableMsg = "Not available";
            public const string Image = "Image";
            public const string Schedule = "Schedule";
            public const string Username = "Username";
            public const string Password = "Password";
            public const string DefaultWeatherIcon = "weathericon.png";
        }

        /// <summary>
        /// Class holding logging constants.
        /// </summary>
        public static class LoggingConstants
        {
            public const string LogFolderName = "LogFolderName";
            public const string LogFileSearchString = "NDTVLogs_*.log";
            public const string DateTimeFormatString = "dd-MMM-yyyy_HH_mm_ss";
            public const char Underscore = '_';
            public const string LogFileNameFormat = "NDTVLogs_{0}.log";
            public const string MaximumLogCount = "MaximumLogCount";
            public const string LogFileExtension = ".log";
            public const string DateTimeLogFormatString = "dd-MMM-yyyy HH:mm:ss";
        }

        /// <summary>
        /// Class holding LiveTV play modes.
        /// </summary>
        public static class LiveTVPlayMode
        {
            public const string PlayTV = "playLive";
            public const string PlayVideoOnDemand = "playVod";
        }

        /// <summary>
        /// All the constants associated with the top_stories module can be found here..
        /// </summary>
        public static class TopStories
        {
            //The Categories received from the NDTV source, used for Mapping an Article catgeory for a Video/Photo Category
            public static class Categories
            {
                public const string TopStory = "Top Stories";
                public const string Recent = "Recent";
                public const string India = "India";
                public const string World = "World";
                public const string Business = "Business";
                public const string Sports = "Sports";
                public const string Cricket = "Cricket";
                public const string Entertainment = "Entertainment";
                public const string Cities = "Cities";
                public const string Technology = "Technology";
                public const string Trending = "Trending";
                public const string Default = "India";
            }

            //The new category items added in the MenuBox..
            public static class NewCategories
            {
                public const string Videos = "Video Gallery";
                public const string Photos = "Photo Gallery";
            }
        }

        public static class FacebookConstants
        {
            /// <summary>
            /// The authorize url.
            /// </summary>
            public const string AuthorizeUrl = "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&display=popup&scope=publish_stream";

            /// <summary>
            /// The access token url.
            /// </summary>
            public const string AccessTokenUrl = "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}";

            /// <summary>
            /// The url from where to get the credentials.
            /// </summary>
            public const string AccessCredentialsUrl = "https://graph.facebook.com/me?access_token={0}&expires={1}&fields=id,name,picture";

            /// <summary>
            /// The url to share data.
            /// </summary>
            public const string ShareUrl = "https://graph.facebook.com/{0}/feed?access_token={1}&expires={2}";

            /// <summary>
            /// The location of the facebook image.
            /// </summary>
            public const string FacebookImageLink = "/Resources/Images/FacebookLogOn.png";

            /// <summary>
            ///  Javascript to inject on home page facebook to for logout.
            /// </summary>
            public const string JavaScriptFunction = "function forceLogout() {{ document.forms['{0}'].submit();}}";
            
            /// <summary>
            ///  Home page text in link of facebook.
            /// </summary>
            public const string FacebookHomoPageText = "home";

            /// <summary>
            ///  Facebook homepage link
            /// </summary>
            public const string FacebookHomePageLink = "https://www.facebook.com/home.php";
        }

        public static class TwitterConstants
        {
            /// <summary>
            /// The url for request token.
            /// </summary>
            public const string RequestTokenLink = "https://api.twitter.com/oauth/request_token";

            /// <summary>
            /// The url for authorization.
            /// </summary>
            public const string AuthorizeUrl = "http://api.twitter.com/oauth/authorize?oauth_token={0}";

            /// <summary>
            /// The url to get the access token.
            /// </summary>
            public const string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";

            /// <summary>
            /// The url to get the credentials.
            /// </summary>
            public const string AccessCredentialsUrl = "http://api.twitter.com/account/verify_credentials.json";

            /// <summary>
            /// The sharing url.
            /// </summary>
            public const string ShareUrl = "http://api.twitter.com/1/statuses/update.json?trim_user=true";

            public const string BitLYShortenUrl = "http://api.bit.ly/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format=json";

            /// <summary>
            /// The location of the twitter image.
            /// </summary>
            public const string TwitterImageLink = "/Resources/Images/TwitterLogOn.png";

            /// <summary>
            /// The url for logout.
            /// </summary>
            public const string LogOffLink = "http://twitter.com/#!/logout";

            /// <summary>
            /// The url for logout.
            /// </summary>
            public const string LogOffSecureLink = "https://twitter.com/#!/logout";

            /// <summary>
            ///  The url whe user click on cancel during signout.
            /// </summary>
            public const string CancelLink = "https://twitter.com/#!/";

            /// <summary>
            ///  The url whe user click on cancel during signout.
            /// </summary>
            public const string LogOffConfirmationText = "logged_out=1";

        }

        /// <summary>
        /// Linked In sharing related constants
        /// </summary>
        public static class LinkedInConstants
        {
            /// <summary>
            /// The url for request token.
            /// </summary>
            public const string RequestTokenLink = "https://api.linkedin.com/uas/oauth/requestToken";

            /// <summary>
            /// The url for authorization.
            /// </summary>
            public const string AuthorizeUrl = "https://api.linkedin.com/uas/oauth/authorize?oauth_token={0}";

            /// <summary>
            /// The url to get the access token.
            /// </summary>
            public const string AccessTokenUrl = "https://api.linkedin.com/uas/oauth/accessToken";

            /// <summary>
            /// The url to get the credentials.
            /// </summary>
            public const string AccessCredentialsUrl = "https://api.linkedin.com/v1/people/~";

            /// <summary>
            /// The url to share data.
            /// </summary>
            public const string ShareUrl = "http://api.linkedin.com/v1/people/~/shares";

            /// <summary>
            /// Linked in xml format to post data to be shared. 
            /// </summary>
            public const string ShareFormat =
                                            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                            + "<share>"
                                            + " <comment>{0}</comment>"
                                            + " <content>"
                                            + "  <title>{1}</title>"
                                            + "  <submitted-url>{2}</submitted-url>"
                                            + "  <submitted-image-url>{3}</submitted-image-url>"
                                            + " </content>"
                                            + " <visibility>"
                                            + "  <code>{4}</code>"
                                            + " </visibility>"
                                            + "</share>";
            
            /// <summary>
            ///  Linked in share visibility type
            /// </summary>
            public static class LinkedInShareVisibilityType 
            {
                /// <summary>
                /// Posted share is visible to anyone
                /// </summary>
                public const string AnyOne = "anyone";

                /// <summary>
                /// Posted share is visible to connections only.
                /// </summary>
                public const string ConnectionsOnly = "connections-only";
            }

            /// <summary>
            /// The location of the LinkedIn image.
            /// </summary>
            public const string LinkedInImageLink = "/Resources/Images/LinkedInLogOn.png";
        }

        public static class GoogleConstants
        {
            /// <summary>
            /// The url for request token.
            /// </summary>
            public const string RequestTokenUrl = "https://www.google.com/accounts/OAuthGetRequestToken?";

            /// <summary>
            /// The post data to be sent, while requesting the request token.
            /// </summary>
            public const string RequestTokenPostData = "https://www-opensocial.googleusercontent.com/api/people/";

            /// <summary>
            /// The url for authorization.
            /// </summary>
            public const string AuthorizeUrl = "https://www.google.com/accounts/OAuthAuthorizeToken?oauth_token={0}&hd=default";

            /// <summary>
            /// The url to get the access token.
            /// </summary>
            public const string AccessTokenUrl = "https://www.google.com/accounts/OAuthGetAccessToken";

            /// <summary>
            /// The url to get the credentials.
            /// </summary>
            public const string GetCredentialsUrl = "https://www-opensocial.googleusercontent.com/api/people/@me/@self";

            /// <summary>
            /// The url for logout.
            /// </summary>
            public const string LogOffLink = "https://www.google.com/accounts/Logout";
        }


        public static class YahooConstants
        {
            /// <summary>
            /// The url for request token.
            /// </summary>
            public const string RequestTokenUrl = "https://api.login.yahoo.com/oauth/v2/get_request_token";

            /// <summary>
            /// The url for authorization.
            /// </summary>
            public const string AuthorizeUrl = "https://api.login.yahoo.com/oauth/v2/request_auth?oauth_token={0}";

            /// <summary>
            /// The url to get the access token.
            /// </summary>
            public const string AccessTokenUrl = "https://api.login.yahoo.com/oauth/v2/get_token";

            /// <summary>
            /// The url to get the credentials.
            /// </summary>
            public const string GetCredentialsUrl = "http://social.yahooapis.com/v1/user/{0}/profile/tinyusercard?format=json";

            /// <summary>
            /// The url for logout.
            /// </summary>
            public const string LogOffLink = "http://login.yahoo.com/config/login?logout=1";

        }

        /// <summary>
        /// Video player notification constants.
        /// </summary>
        public static class VideoPlayerNotificationConstants
        {
            /// <summary>
            ///  Notification message received is Next button
            /// </summary>
            public const string NextButton = "NextButton";

            /// <summary>
            ///  Notification message received is Previous button
            /// </summary>
            public const string PreviousButton = "PreviousButton";

            /// <summary>
            ///  Notification message received is Video end
            /// </summary>
            public const string VideoEnd = "VideoEnd";

        }

        /// <summary>
        /// Comments constants.
        /// </summary>
        public static class CommentConstants
        {
            /// <summary>
            /// Link to retrieve comments.
            /// </summary>
            public const string RetrieveCommentsUrl = "http://jo7oirai.social.ndtv.com/static/Comment/List/?key=ndtvslate1f46079176ee8d40e822bb6&page={0}&identifier=story-{1}&template=json&withpager=1";

            /// <summary>
            /// Link to post a comment.
            /// </summary>
            public const string PostCommentsUrl = "http://jo7oirai.social.ndtv.com/static/Comment/Form/?&key=ndtvslate1f46079176ee8d40e822bb6";

            /// <summary>
            /// Key value pairs post data for comments.
            /// </summary>
            public const string PostCommentPostData = "uid={0}&page_title={1}&page_url={2}&ctype={3}&identifier={4}&comment={5}&submit=Post";
        }

        /// <summary>
        /// Register user constants.
        /// </summary>
        public static class RegisterUserConstants
        {
            /// <summary>
            /// Register user link.
            /// </summary>
            public const string RegisterUserLink = "http://jo7oirai.social.ndtv.com/plugins/index.php?plugin=SocialUserRegister&key=ndtvslate1f46079176ee8d40e822bb6";

            /// <summary>
            /// Register user post data.
            /// </summary>
            public const string RegisterUserData = "user_id={0}&first_name={1}&profile_image={2}&sitename={3}&submit=Save";
        }

        /// <summary>
        /// Contains the request content type
        /// </summary>
        public static class HttpRequestContentType
        {
            /// <summary>
            /// Application/ form url encoded
            /// </summary>
            public const string ApplicationFormLinkEncoded = "application/x-www-form-urlencoded";

            /// <summary>
            /// Text / xml
            /// </summary>
            public const string TextXml = "text/xml";
        }

        /// <summary>
        /// Contains entities related to user feedback.
        /// </summary>
        public static class UserFeedBack
        {
            /// <summary>
            /// Receiver's Email
            /// </summary>
            public const string ReceiversEmail = "ReceiversEmail";

            /// <summary>
            /// Default Sender's Email
            /// </summary>
            public const string DefaultSendersEmail = "DefaultSendersEmail";

            /// <summary>
            /// subject of feedback mail.
            /// </summary>
            public const string Subject = "FeedBack NDTV Slate";
        }
    }
}
