namespace NDTV.Entities
{
    /// <summary>
    /// Http operation which denotes whether it is a get or a post
    /// </summary>
    public enum HttpOperation
    {
        None = 0,
        Get = 1,
        Post
    }

    /// <summary>
    /// This denotes the http authentication associated with a http operation
    /// </summary>
    public enum HttpAuthentication
    {
        None = 0,
        Basic,
        Digest
    }

    /// <summary>
    /// Cricket fixture whether user has selected Live, recent or upcoming tab
    /// </summary>
    public enum CricketFixture
    { 
        Live = 0,
        Recent,
        Upcoming
    }

    /// <summary>
    /// Cricket commentary, whether it is short or detailed
    /// </summary>
    public enum CricketCommentary
    { 
        Short =0,
        Detailed
    }

    /// <summary>
    /// Different types of video attached here..
    /// </summary>
    public enum VideoTypes : int
    {
        Latest=0,
        Category,
        Channel,
        Shows,
        Specials,
        MostViewed,
        MostCommented
    }

    /// <summary>
    /// Video categories.
    /// </summary>
    public enum VideoCategories
    { 
        Featured =0,
        News,
        Business,
        PrimeShows,
        Entertainment,
        Exclusive,
        Environment,
        Sports
    }

    /// <summary>
    /// Image categories.
    /// </summary>
    public enum ImageCategories
    {
        Featured = 0,
        Latest,
        Entertainment,
        News,
        LifeStyle,
        Business,
        Gadgets,
        Health,
        Sports
    }

    /// <summary>
    /// Various forms of ndtv login.
    /// </summary>
    public enum LoginMode
    {
        None = 0,
        Facebook,
        Twitter,
        Google,
        Yahoo,        
    }

    /// <summary> 
    /// Various forms of social interaction. 
    /// </summary> 
    public enum SocialInteractionType
    {
        None = 0,
        Facebook,        
        Twitter,
        LinkedIn,        
        Mail,
        Comments
    }

    /// <summary>
    /// Different forms of share media.
    /// </summary>
    public enum ShareMediaType
    {
        None = 0,
        Video,
        Article,        
        PhotoSet,
        Photo,
        Weather,
        ScorecardLiveMatch,        
        ScorecardRecentMatch,
        LiveCommentary,
        RecentCommentary,
        RecentMatches,
        UpcomingMatches,
    }

    /// <summary>
    /// Various error types
    /// </summary>
    public enum ErrorTypes
    {
        GenericFailure = 0,
        ApplicationLoadFailure,
        ContentLoadFailure,
        ContentShareFailure
    }

    /// <summary>
    /// Different pages available.
    /// </summary>
    public enum Pages
    {
        None = 0,               //0 X 0 
        Home,                   //300 X 250
        CricketLiveCommentary,  //300 X 100
        CricketScorecard,       //748 X 90
        CricketFullCommentary,  //300 X 100
        LiveTV,                 //300 X 100
        NewsItems,              //300 X 100
        PhotoGallerySmall,      //290 X 50 
        PhotoGalleryBig,        //300 X 100
        VideoGallery,           //300 X 100
        Weather,                //300 X 250
        SensexScorecardSmall,   //290 X 50
        SensexScorecardBig      //300 X 250
    }


    /// <summary>
    /// Stock Direction
    /// </summary>
    public enum Direction
    {
        Up,
        Down
    };

    /// <summary>
    /// Search Category Type.
    /// </summary>
    public enum SearchType
    {
        Photos,
        Videos,
        Articles
    };
    

    public enum ArticleType
    {
        MostRead=0,
        MostCommented
    }

}
