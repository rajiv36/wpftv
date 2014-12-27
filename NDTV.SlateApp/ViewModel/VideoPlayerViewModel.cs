using System;
using System.Collections.ObjectModel;
using System.Globalization;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    public class VideoPlayerViewModel:ViewModelBase
    {
        private ObservableCollection<VideoItem> latestVideos;
        private VideoItem videoItem;
        private string videoURL;
        private int videoId;
        private string videoTitle;
        private string publishDate;
        private string duration;
        private string description;

        #region Properties
        public VideoItem Video
        {
            get 
            {
                return videoItem;
            }
            set 
            {
                if (null != value)
                {
                    this.IsLoadingComplete = false;
                    RelatedVideosRequest request = new RelatedVideosRequest(ApplicationData.BuildVideosLink(VideoCategories.Featured));                    
                    ProcessRequest(request, LoadLatestVideosResponse);
                    this.videoItem = value;
                    this.VideoLink = value.VideoFilePath;
                    this.VideoId = value.VideoId;
                    this.VideoTitle = value.Title;
                    this.PublishDate = value.PublishDate;
                    this.Duration = value.Duration;
                    this.Description = value.Description;

                    ApplicationData.SetCurrentItem(value.Title, value.Description, value.VideoId.ToString(CultureInfo.InvariantCulture), 
                        value.ThumbnailLink, value.VideoLink, ShareMediaType.Video);

                    OnPropertyChanged("Video");

                }
            }
        }

        
        /// <summary>
        /// Stores the title of the video
        /// </summary>
        public string VideoTitle
        {
            get { return videoTitle; }
            set 
            {
                videoTitle = value; OnPropertyChanged("VideoTitle"); 
            }
        }

        /// <summary>
        /// Stores the ID of the selected video
        /// </summary>
        public int VideoId
        {
            get { return videoId; }
            set
            {
                videoId = value;
                OnPropertyChanged("VideoId");
            }
        }

        /// <summary>
        /// Stores the information about the selected video
        /// </summary>
        public ObservableCollection<VideoItem> LatestVideos
        {
            get { return latestVideos; }
            set { latestVideos = value; OnPropertyChanged("LatestVideos"); }

        }

        /// <summary>
        /// Stores the selected Video URL
        /// </summary>
        public string VideoLink
        {
            get { return videoURL; }
            set { videoURL = value; OnPropertyChanged("VideoURL"); }
        }

        /// <summary>
        /// Selected videos publish date
        /// </summary>
        public string PublishDate
        {
            get { return publishDate;}
            set { publishDate = value; OnPropertyChanged("PublishDate"); }
        }

        /// <summary>
        /// Duration of the Video
        /// </summary>
        public string Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged("Duration"); }
        }

        /// <summary>
        /// Description of the Video
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        /// <summary>
        /// Loading request
        /// </summary>
        private bool isLoadingComplete;

        /// <summary>
        /// Loading request
        /// </summary>
        public bool IsLoadingComplete
        {
            get { return isLoadingComplete; }
            set 
            {
                isLoadingComplete = value;
                OnPropertyChanged("IsLoadingComplete"); 
            }
        }

        #endregion Properties

        #region Constructor
        /// <summary>
        /// Constructor calls the latest video
        /// </summary>
        /// <param name="video"></param>
        public VideoPlayerViewModel(VideoItem video) 
        {
            this.Video = video;
        }
        #endregion Constructor

        /// <summary>
        /// Response for the latest video
        /// </summary>
        /// <param name="latestVideoResponse"></param>
        protected void LoadLatestVideosResponse(Response latestVideoResponse)
        {
            if (latestVideoResponse.GetType() == typeof(RelatedVideosResponse))
            {
                LatestVideos = (ObservableCollection<VideoItem>)((RelatedVideosResponse)latestVideoResponse).RelatedVideoList;
                IsLoadingComplete = true;
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void DisposeResources()
        {
            Video = null;
            VideoTitle = null;
            LatestVideos = null;
            VideoLink = null;
        }
    }
}
