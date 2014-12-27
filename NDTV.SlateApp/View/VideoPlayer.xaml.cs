using System;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Framework.Utilities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using System.Windows.Media.Animation;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Video Player for Latest videos category.
    /// </summary>
    public partial class VideoPlayer : SlateWindow
    {
        private int videoId;
        private JavaScriptInterOp javaScriptInterOp;

        /// <summary>
        /// Raise the event when video player is closing.
        /// </summary>
        public event EventHandler VideoPlayerClosed;

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor -sets Data context
        /// </summary>
        /// <param name="video"></param>
        public VideoPlayer(VideoItem video)
        {
            InitializeComponent();
            this.SetSize();
            LiveTVVideo.Navigate(Utility.GetLink(Constants.LinkNames.LiveTVVideoPlayerLink));
            VideoPlayerViewModel videoplayer = new VideoPlayerViewModel(video);
            this.DataContext = videoplayer;
            LayoutRoot.DataContext = videoplayer;
            main_pane.DataContext = videoplayer;

            javaScriptInterOp = new JavaScriptInterOp();
            javaScriptInterOp.NotificationReceived += new System.Action<object, string>(JavaScriptInterOpNotificationReceived);
            LiveTVVideo.ObjectForScripting = javaScriptInterOp;

            LiveTVVideo.LoadCompleted += (sender, args) =>
            {

                this.VideoId = videoplayer.VideoId;
                LiveTVVideo.InvokeScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, videoplayer.VideoId);
                JavaScriptInterOp.DisableJavaScriptError(LiveTVVideo);
            };
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
        }

        #endregion Constructor

        #region PEROPERTIES
        /// <summary>
        /// Unique id of the Video file
        /// </summary>
        public int VideoId
        {
            get { return videoId; }
            set { videoId = value; }
        }

        #endregion Properties

        #region PRIVATE METHODS

        /// <summary>
        /// Event handler indicating changed in pop up visibility.
        /// </summary>
        /// <param name="sender">Application Data</param>
        /// <param name="e">Event arguments</param>
        private void IsPopUpOpenValueChanged(object sender, EventArgs e)
        {
            if (ApplicationData.IsPopUpOpen)
            {
                LiveTVVideo.InvokeScript("pauseVideo");
                LiveTVVideo.Visibility = Visibility.Collapsed;
                ModalPopup.Visibility = Visibility.Visible;
                AdControl.ShowAdvertisement = false;
            }
            else
            {
                LiveTVVideo.InvokeScript("playVideo");
                LiveTVVideo.Visibility = Visibility.Visible;
                ModalPopup.Visibility = Visibility.Collapsed;
                AdControl.ShowAdvertisement = true;
            }
        }
        
        /// <summary>
        /// This function notifies the click event of next and 
        /// previous button of video player.
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="message"> message from notification </param>
        private void JavaScriptInterOpNotificationReceived(object sender, string message)
        {
            if (false == String.IsNullOrEmpty(message))
            {
                if (message.Equals(Constants.VideoPlayerNotificationConstants.NextButton))
                {
                    PlayNextVideo();
                }
                else if (message.Equals(Constants.VideoPlayerNotificationConstants.PreviousButton))
                {
                    PlayPreviousVideo();
                }
                else if (message.Equals(Constants.VideoPlayerNotificationConstants.VideoEnd))
                {
                    PlayNextVideo();
                }
            }
        }

        /// <summary>
        /// Play next video.
        /// </summary>
        private void PlayNextVideo()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                int nextIndex = RelatedVideoList.SelectedIndex + 1;

                if ((nextIndex > -1) && (nextIndex < RelatedVideoList.Items.Count))
                {
                    RelatedVideoList.SelectedIndex = nextIndex;
                    VideoItem selectedVideo = RelatedVideoList.SelectedItem as VideoItem;
                    if (null != selectedVideo)
                    {
                        LiveTVVideo.Visibility = Visibility.Hidden;
                        VideoPlayerViewModel videoPlayer = new VideoPlayerViewModel(selectedVideo);
                        this.DataContext = videoPlayer;
                        LayoutRoot.DataContext = videoPlayer;
                        this.VideoId = videoPlayer.VideoId;
                        LiveTVVideo.InvokeScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, videoPlayer.VideoId);
                        LiveTVVideo.Visibility = Visibility.Visible;
                        this.AdControl.RefreshAdBanner();
                    }
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Play previous video.
        /// </summary>
        private void PlayPreviousVideo()
        {
            if (ApplicationData.IsApplicationOnline)
            {
                int previouseIndex = RelatedVideoList.SelectedIndex - 1;

                if ((previouseIndex > -1) && (previouseIndex < RelatedVideoList.Items.Count))
                {
                    RelatedVideoList.SelectedIndex = previouseIndex;
                    VideoItem selectedVideo = RelatedVideoList.SelectedItem as VideoItem;

                    if (null != selectedVideo)
                    {
                        LiveTVVideo.Visibility = Visibility.Hidden;
                        VideoPlayerViewModel videoPlayer = new VideoPlayerViewModel(selectedVideo);
                        this.DataContext = videoPlayer;
                        LayoutRoot.DataContext = videoPlayer;
                        this.VideoId = videoPlayer.VideoId;
                        LiveTVVideo.InvokeScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, videoPlayer.VideoId);
                        LiveTVVideo.Visibility = Visibility.Visible;
                        this.AdControl.RefreshAdBanner();
                    }
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Get the details of the selcted Video file 
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="args">Event arguments</param>       
        private void VideoListSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            StopVideo();
            if (ApplicationData.IsApplicationOnline)
            {

                VideoItem selectedVideo = RelatedVideoList.SelectedItem as VideoItem;
                if (selectedVideo != null)
                {
                    LiveTVVideo.Visibility = Visibility.Hidden;
                    VideoPlayerViewModel videoPlayer = new VideoPlayerViewModel(selectedVideo);
                    this.DataContext = videoPlayer;
                    LayoutRoot.DataContext = videoPlayer;
                    main_pane.DataContext = videoPlayer;

                    this.VideoId = videoPlayer.VideoId;
                    LiveTVVideo.InvokeScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, videoPlayer.VideoId);
                    LiveTVVideo.Visibility = Visibility.Visible;
                    this.AdControl.RefreshAdBanner();
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Stoping video and audio stream.
        /// </summary>
        private void StopVideo()
        {
            try
            {
                LiveTVVideo.InvokeScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
            }
            catch (System.Runtime.InteropServices.COMException exception)
            {
                ApplicationData.ErrorLogger.Log(exception);
            }
        }

        /// <summary>
        /// This responds to close event. 
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"></param>
        private void OnVideoPlayerCloseButtonClick(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            /* Stoping continuous audio streaming after close event. */
            StopVideo();

            if (null != VideoPlayerClosed)
            {
                /* Delegate close event to the host of this control. */
                VideoPlayerClosed(sender, e);
            }

            this.Close();            
        }

        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            this.SetWindowPosition();
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potrait Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortraitView();
            this.SetWindowPosition();
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                LandscapeView();
                this.SetWindowPosition();

            }
            else
            {
                PortraitView();
                this.SetWindowPosition();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            live_tv_player.Height = double.NaN;
            PlayerColumn.Width = new GridLength(550, GridUnitType.Pixel);
            RelatedVideosColumn.Width = new GridLength(1, GridUnitType.Star);
            PlayerRow.Height = new GridLength(1, GridUnitType.Star);
            RelatedVideosRow.Height = GridLength.Auto;
            RightPane.SetValue(Grid.RowProperty, 0);
            RightPane.SetValue(Grid.ColumnProperty, 1);
            RelatedVideoList.ScrollIntoView(RelatedVideoList.SelectedItem);
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
        }

        /// <summary>
        /// A Method which sets the window to Potrait orientation with all the required properties
        /// </summary>
        private void PortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            live_tv_player.Height = 400;
            PlayerColumn.Width = new GridLength(1, GridUnitType.Star);
            RelatedVideosColumn.Width = new GridLength(0, GridUnitType.Pixel);
            PlayerRow.Height = GridLength.Auto;
            RelatedVideosRow.Height = new GridLength(1, GridUnitType.Star);
            RightPane.SetValue(Grid.RowProperty, 1);
            RightPane.SetValue(Grid.ColumnProperty, 0);
            RelatedVideoList.ScrollIntoView(RelatedVideoList.SelectedItem);
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
        }

        /// <summary>
        /// Sets the window position calculating the resolution of the window.
        /// </summary>
        private void SetWindowPosition()
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
        }

        #endregion



    }
}
