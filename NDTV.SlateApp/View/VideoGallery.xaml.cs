using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using System.Windows.Media.Animation;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Video gallery window.
    /// </summary>
    public partial class VideoGallery : SlateWindow, IDisposable
    {
        private VideoGalleryViewModel currentVideoGalleryViewModel = null;
        private VideoPlayerControl videoPlayerControl = null;
        private bool isFirstTimeVideoPlayerLoading;

        #region CONSTRUCTOR

        /// <summary>
        /// Default constructor, intialize controls, and members.
        /// </summary>
        public VideoGallery()
        {
            
            InitializeComponent();            
            isFirstTimeVideoPlayerLoading = true;
            
            LoadVideosForCategory(ApplicationData.Settings.VideoAlbumCarouselCategory);
            
            /* Wire up online and offline event from application */
            App application = App.Current as App;
            if (null != application)
            {
                application.OnApplicationOffline += new EventHandler(OnConnectionLoss);
                application.OnApplicationOnline += new EventHandler(OnConnectionReceived);
            }
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
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
                ModalPopup.Visibility = Visibility.Visible;
            }
            else
            {                
                ModalPopup.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// Binding Visual controls.
        /// </summary>
        private void BindingControls()
        {
            this.DataContext = currentVideoGalleryViewModel;
            this.VideoGalleryControl.ItemsSource = currentVideoGalleryViewModel.VideoList;
        }

        /// <summary>
        /// Starts loading video items for selected category.
        /// </summary>
        /// <param name="selectedCategory"></param>
        private void LoadVideosForCategory(string selectedCategory)
        {
            this.AdControl.ShowAdvertisement = false;
            /* Dispose previous view model object if created */
            if (null != currentVideoGalleryViewModel)
            {
                currentVideoGalleryViewModel.Dispose();
            }

            this.currentVideoGalleryViewModel = new VideoGalleryViewModel(selectedCategory);
            ApplicationData.Settings.VideoAlbumCarouselCategory = selectedCategory; 
           
            /* Retrieve videos for selected category */
            this.currentVideoGalleryViewModel.VideoGalleryCommand.Execute(currentVideoGalleryViewModel.CurrentVideoCategory);                
            BindingControls();
            this.AdControl.ShowAdvertisement = true;
        }

        /// <summary>
        /// This Event handler responds to category click event and load
        /// the selected categroy into video control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategoryClick(object sender, RoutedEventArgs e)
        {            
            if (NDTV.Controller.ApplicationData.IsApplicationOnline)
            {
                if (null != currentVideoGalleryViewModel)
                {
                    string radioButtonContent = (sender as RadioButton).Content.ToString();
                    /* Get Content of Checked radio button */                    
                    string selectedCategoryString = radioButtonContent;
                    LoadVideosForCategory(selectedCategoryString);                    
                }
                if (ApplicationData.IsLandscapeOrientation)
                {
                    this.AdControl.Visibility = Visibility.Visible;
                    this.adBannerControlBig.Visibility = Visibility.Collapsed;
                    this.adBannerControlSmall.Visibility = Visibility.Collapsed;
                    this.AdControl.RefreshAdBanner();
                }
                else
                {
                    this.AdControl.Visibility = Visibility.Hidden;
                    this.adBannerControlBig.Visibility = Visibility.Visible;
                    this.adBannerControlSmall.Visibility = Visibility.Visible;
                    this.adBannerControlBig.RefreshAdBanner();
                    this.adBannerControlSmall.RefreshAdBanner();
                }
                
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Loads video player.
        /// </summary>
        /// <param name="videoItem">Video related details</param>
        private void LoadVideoPlayer(VideoItem videoItem)
        {
            if (NDTV.Controller.ApplicationData.IsApplicationOnline)
            {
                if (null != videoItem)
                {
                    if (null != videoPlayerControl)
                    {
                        videoPlayerControl.PlayVideo(videoItem);
                    }
                    else
                    {
                        videoPlayerControl = new VideoPlayerControl(videoItem);
                        videoPlayerControl.VideoPlayerClosed += new EventHandler(OnVideoPlayerClosed);
                        videoPlayerControl.NextVideoButtonClick += new EventHandler(OnNextVideoButtonClick);
                        videoPlayerControl.PreviousVideoButtonClick += new EventHandler(OnPreviousVideoButtonClick);
                    }
                    ShowVideoPlayer();
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Hides video player and shows video gallery and category list.
        /// </summary>
        private void ShowVideoGallery()
        {
            this.videoPlayerControl.Visibility = Visibility.Collapsed;
            this.VideoCategoryButtons.Visibility = Visibility.Visible;
            this.FlowControlRoot.Visibility = Visibility.Visible;
            //this.AdControl.ShowAdverticement = true;
        }

        /// <summary>
        /// Hides video gallery and category list, and shows the video player.
        /// </summary>
        private void ShowVideoPlayer()
        {
            //this.AdControl.ShowAdverticement = false;
            this.FlowControlRoot.Visibility = Visibility.Collapsed;
            this.VideoCategoryButtons.Visibility = Visibility.Hidden;
            /* The video player will be added only once, in a grid. */
            if (isFirstTimeVideoPlayerLoading)
            {
                this.VideoContainer.Children.Add(videoPlayerControl);
                isFirstTimeVideoPlayerLoading = false;
            }
            this.videoPlayerControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// This event responds to click event on Image, and open the video player 
        /// control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoGalleryControllOnClick(object sender, MouseButtonEventArgs e)
        {
            string imagePath = ((System.Windows.Controls.Image)(e.OriginalSource)).Source.ToString();
            VideoItem videoItem = (VideoItem)VideoGalleryControl.SelectedItem;

            /* Shall open video player only if clicked on center image */
            if (null != videoItem && videoItem.ThumbnailLinkLarge.Equals(imagePath))
            {
                LoadVideoPlayer(videoItem);
            }
        }

        /// <summary>
        /// This event handler response to close button click event of the video gallery.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            /* 
             * Stop audio if video gallery is closed directly 
             *  without closing videoplayer.
             */
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            if (null != videoPlayerControl)
            {
                videoPlayerControl.StopVideo();
                videoPlayerControl.Dispose();
            }
            this.Close();
        }

        /// <summary>
        /// This event handler response to close button click event of the video player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVideoPlayerClosed(object sender, EventArgs args)
        {            
            ShowVideoGallery();

            this.AdControl.RefreshAdBanner();
            this.adBannerControlBig.RefreshAdBanner();
            this.adBannerControlSmall.RefreshAdBanner();

        }

        /// <summary>
        /// Playing next video if user clicked on video player control next button.
        /// </summary>
        /// <param name="sender"> The event sender object. </param>
        /// <param name="args"> The event arguments. </param>
        private void OnNextVideoButtonClick(object sender, EventArgs args)
        {
            PlayNextVideo();
        }

        /// <summary>
        /// Playing previous video if user clicked on video player control next button.
        /// </summary>
        /// <param name="sender"> The event sender object. </param>
        /// <param name="args"> The event arguments. </param>
        private void OnPreviousVideoButtonClick(object sender, EventArgs args)
        {
            PlayPreviousVideo();
        }

        /// <summary>
        /// Play next video.
        /// </summary>
        private void PlayNextVideo()
        {
            if (null != this.videoPlayerControl)
            {
                int nextVideoIndex = -1;
                nextVideoIndex = this.VideoGalleryControl.SelectedIndex + 1;
                if (nextVideoIndex > -1 && nextVideoIndex < this.VideoGalleryControl.Items.Count)
                {
                    this.VideoGalleryControl.SelectedIndex = nextVideoIndex;
                    this.videoPlayerControl.PlayVideo((VideoItem)VideoGalleryControl.SelectedItem);
                    ShowVideoPlayer();
                }
            }
        }

        /// <summary>
        /// Play previous video.
        /// </summary>
        private void PlayPreviousVideo()
        {
            if (null != this.videoPlayerControl)
            {
                int previousVideoIndex = -1;
                previousVideoIndex = this.VideoGalleryControl.SelectedIndex - 1;
                if (previousVideoIndex > -1 && previousVideoIndex < this.VideoGalleryControl.Items.Count)
                {
                    this.VideoGalleryControl.SelectedIndex = previousVideoIndex;
                    this.videoPlayerControl.PlayVideo((VideoItem)VideoGalleryControl.SelectedItem);
                    ShowVideoPlayer();
                }
            }
        }

        /// <summary>
        /// Responds when application goes offline.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="arguments">Event args Argument </param>
        private void OnConnectionLoss(Object sender, EventArgs arguments)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.CategoriesList.IsEnabled = false;
            }));
            
        }

        /// <summary>
        /// Responds when application comes online.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="arguments">Event args Argument </param>
        private void OnConnectionReceived(Object sender, EventArgs arguments)
        {

            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.CategoriesList.IsEnabled = true;
                string selectedCategory = ApplicationData.Settings.VideoAlbumCarouselCategory;
                if (null != selectedCategory)
                {
                    /* Dispose previous view model object if created */
                    if (null != currentVideoGalleryViewModel)
                    {
                        currentVideoGalleryViewModel.Dispose();
                    }

                    currentVideoGalleryViewModel = new VideoGalleryViewModel(selectedCategory);

                    /* Retrieve videos for selected category */
                    currentVideoGalleryViewModel.VideoGalleryCommand.Execute(currentVideoGalleryViewModel.CurrentVideoCategory);
                    BindingControls();
                }
                this.AdControl.RefreshAdBanner();
                this.adBannerControlBig.RefreshAdBanner();
                this.adBannerControlSmall.RefreshAdBanner();
            }));
        }


        /// <summary>
        /// Overriding the method where in the grids are Visibility controlled here..
        /// Potriat Container is hidden and Landscape Container is brought to focus
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            LandscapeView();
            this.SetWindowPosition();
            if (null != videoPlayerControl)
            {
                videoPlayerControl.SetOrientation();
            }
        }

        /// <summary>
        ///  Overriding the method where in the grids are Visibility controlled here..
        ///  Landscape Container is hidden and Potrait Container is brought to focus
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            PortraitView();
            this.SetWindowPosition();
            if (null != videoPlayerControl)
            {
                videoPlayerControl.SetOrientation();
            }
        }

        /// <summary>
        /// The overriden method which sets the size of the window based on whether it is in landscape or potriat version
        /// </summary>
        protected override void SetSize()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                this.SetWindowPosition();
                LandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                PortraitView();
            }
            if (null != videoPlayerControl)
            {
                videoPlayerControl.SetOrientation();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            PotraitGrid.Visibility = Visibility.Collapsed;
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();
            this.AdControl.Visibility = Visibility.Visible;
            this.adBannerControlBig.Visibility = Visibility.Collapsed;
            this.adBannerControlSmall.Visibility = Visibility.Collapsed;
            this.AdControl.RefreshAdBanner();
            
        }

        /// <summary>
        /// A Method which sets the window to Potrait orientation with all the required properties
        /// </summary>
        private void PortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            PotraitGrid.Visibility = Visibility.Visible;
            Storyboard sb = (Storyboard)TryFindResource("PotraitLandscapeAnimation");
            sb.Begin();

            this.AdControl.Visibility = Visibility.Hidden;
            this.adBannerControlBig.Visibility = Visibility.Visible;
            this.adBannerControlSmall.Visibility = Visibility.Visible;
            this.adBannerControlBig.RefreshAdBanner();
            this.adBannerControlSmall.RefreshAdBanner();
            
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

        #region PUBLIC METHODS

        /// <summary>
        /// Disposing managed resources.
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
        /// <param name="disposing"> To indicate if dispose required </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                /* free managed resources */
                if (null != this.currentVideoGalleryViewModel)
                {
                    this.currentVideoGalleryViewModel.Dispose();
                    this.currentVideoGalleryViewModel = null;
                }

                if (null != this.videoPlayerControl)
                {
                    this.videoPlayerControl.VideoPlayerClosed -= new EventHandler(OnVideoPlayerClosed);
                    this.videoPlayerControl.NextVideoButtonClick -= new EventHandler(OnNextVideoButtonClick);
                    this.videoPlayerControl.PreviousVideoButtonClick -= new EventHandler(OnPreviousVideoButtonClick);

                    this.videoPlayerControl.Dispose();
                    this.videoPlayerControl = null;
                }
            }
        }

        #endregion 
    }
}