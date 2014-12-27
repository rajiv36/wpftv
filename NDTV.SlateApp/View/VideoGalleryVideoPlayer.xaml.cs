using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using System.Windows.Controls;


namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for LiveTVVideoPlayer.xaml
    /// </summary>
    public partial class VideoGalleryVideoPlayer : SlateWindow
    {
        private int videoId;

        #region Properties
        /// <summary>
        /// Unique id of the Video file
        /// </summary>
        public int VideoId
        {
            get { return videoId; }
            set { videoId = value;  }

        }
        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor -sets Data context
        /// </summary>
        /// <param name="video"></param>
        public VideoGalleryVideoPlayer(VideoItem video)
        {
            InitializeComponent();
            LiveTVVideo.Navigate(Utility.GetLink(Constants.LinkNames.LiveTVVideoPlayerLink));
            VideoPlayerViewModel videoplayer= new VideoPlayerViewModel(video);
            this.DataContext = videoplayer;
            LayoutRoot.DataContext = videoplayer;
            VideoBrowserContainer.DataContext = videoplayer;
            
            LiveTVVideo.LoadCompleted += (sender, args) =>
                {
                    LiveTVVideo.InvokeScript("playVod", videoplayer.VideoId);
                };
            
        }
        #endregion Constructor




        /// <summary>
        /// Get the details of the selcted Video file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string imagepath = string.Empty;
            ObservableCollection<VideoItem> video = ((ObservableCollection<VideoItem>)((System.Windows.Controls.ItemsControl)(sender)).ItemsSource);
            if (e.OriginalSource.GetType().ToString().Substring(0).Contains("Image"))
            {
                imagepath = ((System.Windows.Controls.Image)(e.OriginalSource)).Source.ToString();
                List<VideoItem> Videopath = new List<VideoItem>();
                Videopath = (from item in video.ToList()
                             where item.ThumbnailLink == imagepath
                             select new VideoItem() 
                             {
                                 ThumbnailLink = item.ThumbnailLink,
                                 Description = item.Description,
                                 Title = item.Title,
                                 VideoFilePath = item.VideoFilePath,
                                 VideoId = item.VideoId,
                                 PublishDate = item.PublishDate,
                                 Duration = item.Duration
                             }).ToList();
                if(ApplicationData.IsApplicationOnline)
                {
                    VideoPlayerViewModel videoPlayer = new VideoPlayerViewModel(Videopath[0]);
                    this.DataContext = videoPlayer;
                    LayoutRoot.DataContext = videoPlayer;
                    LiveTVVideo.InvokeScript("playVod", videoPlayer.VideoId);
                }
                 else
                {
                    (App.Current as App).DisplayErrorMessage( NDTV.SlateApp.Properties.Resources.GeneralFailureMessage, string.Empty, false, null);
                }
            }
        }


        /// <summary>
        /// Closes this Video Player Popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
