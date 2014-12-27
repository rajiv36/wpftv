using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.Framework.Utilities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;


namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Video Player, user control.
    /// </summary>
    public partial class VideoPlayerControl : UserControl, IDisposable
    {
        private VideoPlayerViewModel playerViewModel = null;
        private JavaScriptInterOp javaScriptInterOp = null;

        /// <summary>
        /// Event that responds to Next Video Button click.
        /// </summary>
        public event EventHandler NextVideoButtonClick;
        
        /// <summary>
        /// Event that responds to Previous Video Button click.
        /// </summary>
        public event EventHandler PreviousVideoButtonClick;

        /// <summary>
        /// Event variable to create layer on delegate.
        /// </summary>
        public event EventHandler VideoPlayerClosed;

        /// <summary>
        /// This event notifies if video ends playing
        /// </summary>
        public event EventHandler VideoEnd;

        #region CONSTRUCTOR

        /// <summary>
        /// Set up video player to play video specified by the parameter "video".
        /// </summary>
        /// <param name="video"> Video to be played. </param>
        public VideoPlayerControl(VideoItem video)
        {
            InitializeComponent();
            SetOrientation();

            NdtvVideoPlayer.Navigate(Utility.GetLink(Constants.LinkNames.LiveTVVideoPlayerLink));

            javaScriptInterOp = new JavaScriptInterOp();
            javaScriptInterOp.NotificationReceived += 
                new Action<object, string>(JavaScriptInterOpOnNotificationReceived);
            NdtvVideoPlayer.ObjectForScripting = javaScriptInterOp;
            
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;

            PlayVideo(video);

        }

        /// <summary>
        /// Set orientation to the user control.
        /// </summary>
        public void SetOrientation()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.adBanner.Visibility = Visibility.Collapsed;
                adBannerControlSmall.Visibility = Visibility.Collapsed;
                adBannerControlBig.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.adBanner.Visibility = Visibility.Visible;
                adBannerControlSmall.Visibility = Visibility.Visible;
                adBannerControlBig.Visibility = Visibility.Visible;
                this.adBannerControlSmall.RefreshAdBanner();
                this.adBannerControlBig.RefreshAdBanner();
            }
        }
        #endregion

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
                NdtvVideoPlayer.InvokeScript("pauseVideo");
                NdtvVideoPlayer.Visibility = Visibility.Collapsed;
                ModalPopup.Visibility = Visibility.Visible;
            }
            else
            {
                NdtvVideoPlayer.InvokeScript("playVideo");
                NdtvVideoPlayer.Visibility = Visibility.Visible;
                ModalPopup.Visibility = Visibility.Collapsed;
            }
        }


        /// <summary>
        /// Notifies the click event of next and previous button of video player.
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="message"> message from notification </param>
        private void JavaScriptInterOpOnNotificationReceived(object sender, string message)
        {
            if (false == String.IsNullOrEmpty(message))
            {
                if (message.Equals(Constants.VideoPlayerNotificationConstants.NextButton))
                {
                    OnNextVideoButtonClick(new EventArgs());
                }
                else if (message.Equals(Constants.VideoPlayerNotificationConstants.PreviousButton))
                {
                    OnPreviousVideoButtonClick(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Seting data context and binding controls.
        /// </summary>     
        private void BindingControls()
        {
            this.DataContext = playerViewModel;
        }

        /// <summary>
        /// Closing video player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVideoPlayerCloseButtonClick(object sender, RoutedEventArgs e)
        {
            /* Stoping continuous audio streaming after close event. */
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            StopVideo();
            if (null != VideoPlayerClosed)
            {
                /* Delegate close event to the host of this control. */
                VideoPlayerClosed(sender, e);
            }
        }

        private void NdtvVideoPlayerLoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.SocialSharingButtons.IsEnabled = true;
        }

        #endregion

        #region PUBLIC METHDOS
        

        /// <summary>
        /// Start playing video.
        /// </summary>
        /// <param name="videoId"> Unique video id. </param>
        public void PlayVideo(VideoItem video)
        {
            /* dispose player view model data before you get new model */
            if (null != playerViewModel)
            {
                playerViewModel.Dispose();
            }
            playerViewModel = new VideoPlayerViewModel(video);

            BindingControls();

            try
            {
                /* If player content is loaded invoke java script function */
                if (NdtvVideoPlayer.IsLoaded)
                {
                    NdtvVideoPlayer.InvokeScript("playVod", playerViewModel.VideoId);
                    JavaScriptInterOp.DisableJavaScriptError(NdtvVideoPlayer);
                }
                else
                {
                    /* If player is still loading wait till it loads and then invok java script function */
                    NdtvVideoPlayer.LoadCompleted += (sender, args) =>
                    {
                        NdtvVideoPlayer.Visibility = Visibility.Hidden;
                        NdtvVideoPlayer.InvokeScript("playVod", playerViewModel.VideoId);
                        JavaScriptInterOp.DisableJavaScriptError(NdtvVideoPlayer);
                        NdtvVideoPlayer.Visibility = Visibility.Visible;
                    };
                }
            }
            catch (COMException exception)
            {
                /* Handle java script function not found exception */
                ApplicationData.ErrorLogger.Log(exception);
            }
        }

        /// <summary>
        /// Stoping audio and video. This helps in Close events of video gallary and player. 
        /// </summary>
        public void StopVideo()
        {
            try
            {
                /* Stoping continuous audio streaming by invoking video plyer with empty string. */
                NdtvVideoPlayer.InvokeScript("playVod", String.Empty);
                JavaScriptInterOp.DisableJavaScriptError(NdtvVideoPlayer);
            }
            catch (COMException exception)
            {
                ApplicationData.ErrorLogger.Log(exception);
            }
        }

        /// <summary>
        /// Disposing video player resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region PROTECTED METHODS

        /// <summary>
        /// The bulk of the clean-up code is implemented in Dispose(bool)
        /// </summary>
        /// <param name="disposing"> To indicate if dispose required for managed resource. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                /* free managed resources */
                if (null != this.playerViewModel)
                {
                    this.playerViewModel.Dispose();
                    this.playerViewModel = null;

                    if (null != javaScriptInterOp)
                    {
                        this.javaScriptInterOp.NotificationReceived -=
                            new Action<object, string>(JavaScriptInterOpOnNotificationReceived);
                        this.javaScriptInterOp = null;
                    }
                }
            }
        }

        /// <summary>
        /// Raise event to play next video.
        /// </summary>
        /// <param name="argument"></param>
        protected virtual void OnNextVideoButtonClick(EventArgs argument)
        {
            if (null != NextVideoButtonClick)
            {
                NextVideoButtonClick(this, argument);
            }
        }

        /// <summary>
        /// Raise event to play previous video.
        /// </summary>
        /// <param name="argument"> Event arguments </param>
        protected virtual void OnPreviousVideoButtonClick(EventArgs argument)
        {
            if (null != PreviousVideoButtonClick)
            {
                PreviousVideoButtonClick(this, argument);
            }
        }

        /// <summary>
        /// Raise event when video ends.
        /// </summary>
        /// <param name="argument"></param>
        protected virtual void OnVideoEnd(EventArgs argument)
        {
            if (null != VideoEnd)
            {
                VideoEnd(this, argument);
            }
        }

        #endregion



    }
}
