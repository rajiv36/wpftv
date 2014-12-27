using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for LiveTVView.xaml
    /// </summary>
    public partial class LiveTVView : SlateWindow
    {
          
        private ChannelScheduleViewModel channelScheduleViewModel;
        private bool isLoadCompleted;
        private bool isFirstTime;
        
        # region Properties
        /// <summary>
        /// Holds the selecte channel button
        /// </summary>
        public string CurrentChannel
        {
            get;
            set;
        }

        /// <summary>
        /// Checks whether the Live TV video player is loaded
        /// </summary>
        public bool IsLoadCompleted
        {
            get
            {
                return isLoadCompleted;
            }
            set
            {
                isLoadCompleted = value;
            }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Default constructor. creates the button click events and loads the NDTV24x7 channel
        /// </summary>
        public LiveTVView()
        {
            isLoadCompleted = false;

            InitializeComponent();
            
            isFirstTime = true;
            channelScheduleViewModel = new ChannelScheduleViewModel();
            LiveTVBrowser.Navigate(Utility.GetLink(Constants.LinkNames.LiveTVVideoPlayerLink));
            this.LiveTVBrowser.LoadCompleted += (s, args) =>
            {                              
                SetCurrentChannel();
                
            };

            if (null != App.Current)
            {
                (App.Current as App).OnApplicationOnline += OnApplicationOnline;
                (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            }
           this.CurrentChannel = Constants.LinkNames.LiveTVNDTV24x7Schedule;

            
            GrLiveTV.DataContext = channelScheduleViewModel;
            GrSchedule.DataContext = channelScheduleViewModel;
            stkSchedule.DataContext = channelScheduleViewModel;
           
           channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTV24x7Schedule);            
            this.btnGoodTimes.Click += (s, args) =>
             {

                 if (ApplicationData.IsApplicationOnline)
                 {
                     if (this.CurrentChannel != Constants.LinkNames.LiveTVNDTVGoodTimesSchedule)
                     {
                         
                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
                         channelScheduleViewModel = new ChannelScheduleViewModel();
                         channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTVGoodTimesSchedule);
                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, Constants.LinkNames.LiveTVGoodTimesId);
                         
                         GrSchedule.DataContext = channelScheduleViewModel;
                         stkSchedule.DataContext = channelScheduleViewModel;
                         this.DataContext = channelScheduleViewModel;
                         this.CurrentChannel = Constants.LinkNames.LiveTVNDTVGoodTimesSchedule;
                         ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTVGoodTimesSchedule;
                     }
                 }
                 else
                 {
                     (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                 }
             };
            this.btnIndia.Click += (s, args) =>
             {
                 if (ApplicationData.IsApplicationOnline)
                 {
                     if (this.CurrentChannel != Constants.LinkNames.LiveTVNDTVIndiaSchedule)
                     {

                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
                         channelScheduleViewModel = new ChannelScheduleViewModel();
                         channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTVIndiaSchedule);
                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, Constants.LinkNames.LiveTVIndiaId);
                         
                         GrSchedule.DataContext = channelScheduleViewModel;
                         stkSchedule.DataContext = channelScheduleViewModel;
                         
                         this.CurrentChannel = Constants.LinkNames.LiveTVNDTVIndiaSchedule;
                         ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTVIndiaSchedule;
                     }
                 }
                 else
                 {
                     (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                 }
             };
            this.btnNDTV24.Click += (s, args) =>
             {
                 if (ApplicationData.IsApplicationOnline)
                 {
                     if (this.CurrentChannel != Constants.LinkNames.LiveTVNDTV24x7Schedule)
                     {

                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
                         channelScheduleViewModel = new ChannelScheduleViewModel();
                         channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTV24x7Schedule);
                         InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, Constants.LinkNames.LiveTV24x7Id);
                         GrSchedule.DataContext = channelScheduleViewModel;
                         stkSchedule.DataContext = channelScheduleViewModel;
                         this.CurrentChannel = Constants.LinkNames.LiveTVNDTV24x7Schedule;
                         ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTV24x7Schedule;
                     }
                 }
                 else
                 {
                     (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                 }
             };
            this.btnProfit.Click += (s, args) =>
                {
                    if (ApplicationData.IsApplicationOnline)
                    {
                        if (this.CurrentChannel != Constants.LinkNames.LiveTVNDTVProfitSchedule)
                        {
                            InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
                            this.CurrentChannel = Constants.LinkNames.LiveTVNDTVProfitSchedule;
                            channelScheduleViewModel = new ChannelScheduleViewModel();
                            channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTVProfitSchedule);
                            InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, Constants.LinkNames.LiveTVProfitId);
                            GrSchedule.DataContext = channelScheduleViewModel;
                            stkSchedule.DataContext = channelScheduleViewModel;
                            this.CurrentChannel = Constants.LinkNames.LiveTVNDTVProfitSchedule;
                            ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTVProfitSchedule;
                        }
                 }
                  else
                  {
                      (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                  }
                };

            this.btnHindu.Click += (s, args) =>
            {
                if (ApplicationData.IsApplicationOnline)
                {
                    if (this.CurrentChannel != Constants.LinkNames.LiveTVNDTVHinduSchedule)
                    {

                        InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
                        channelScheduleViewModel = new ChannelScheduleViewModel();
                        channelScheduleViewModel.CurrentShow = Constants.Constant.NotAvailableMsg;
                        channelScheduleViewModel.UpcomingShow = Constants.Constant.NotAvailableMsg;
                        channelScheduleViewModel.ChannelScheduleCommand.Execute(Constants.LinkNames.LiveTVNDTVHinduSchedule);
                        InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, Constants.LinkNames.LiveTVHinduId);
                        GrSchedule.DataContext = channelScheduleViewModel;
                        stkSchedule.DataContext = channelScheduleViewModel;
                        this.CurrentChannel = Constants.LinkNames.LiveTVNDTVHinduSchedule;
                        ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTVHinduSchedule;
                    }
                }
                 else
                 {
                     (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
                 }
            };
           
        }

        #endregion Constructor

        #region Private methods

        /// <summary>
        /// Event fires when application comes online.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnApplicationOnline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ChannelButtons.IsEnabled = true;
            }));
        }

        /// <summary>
        /// Event fires when application comes offline.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args</param>
        private void OnApplicationOffline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                ChannelButtons.IsEnabled = false;
            }));
        }

        /// <summary>
        /// This method sets the current channel based on the settings entry
        /// </summary>
        private void SetCurrentChannel()
        {
            if (string.IsNullOrEmpty(ApplicationData.Settings.LiveTVChannel))
            {
                btnNDTV24.IsChecked = true;
                PlayChannel(Constants.LinkNames.LiveTVNDTV24x7Schedule, Constants.LinkNames.LiveTV24x7Id);
                return;
            }

            this.CurrentChannel = ApplicationData.Settings.LiveTVChannel;
            string channelPlayerId = string.Empty;

            switch (ApplicationData.Settings.LiveTVChannel)
            {
                case Constants.LinkNames.LiveTVNDTVGoodTimesSchedule:
                    btnGoodTimes.IsChecked = true;
                    channelPlayerId = Constants.LinkNames.LiveTVGoodTimesId;
                    break;
                case Constants.LinkNames.LiveTVNDTVIndiaSchedule:
                    btnIndia.IsChecked = true;
                    channelPlayerId = Constants.LinkNames.LiveTVIndiaId;
                    break;
                case Constants.LinkNames.LiveTVNDTVHinduSchedule:
                    btnHindu.IsChecked = true;
                    channelPlayerId = Constants.LinkNames.LiveTVHinduId;
                    break;
                case Constants.LinkNames.LiveTVNDTVProfitSchedule:
                    btnProfit.IsChecked = true;
                    channelPlayerId = Constants.LinkNames.LiveTVProfitId;
                    break;
                default:
                    btnNDTV24.IsChecked = true;
                    this.CurrentChannel = Constants.LinkNames.LiveTVNDTV24x7Schedule;
                    ApplicationData.Settings.LiveTVChannel = Constants.LinkNames.LiveTVNDTV24x7Schedule;
                    channelPlayerId = Constants.LinkNames.LiveTV24x7Id;
                    break;
            }
            PlayChannel(ApplicationData.Settings.LiveTVChannel, channelPlayerId);
            isFirstTime = false;
        }

        /// <summary>
        /// Plays the given channel
        /// </summary>
        /// <param name="channelId">Channel Id</param>
        /// <param name="channelPlayerID">The Id to play the channel</param>
        private void PlayChannel(string channelId, string channelPlayerID)
        {            
            if (ApplicationData.IsApplicationOnline)
            {                
                if (isFirstTime || false == this.CurrentChannel.Equals(channelId))
                {
                    channelScheduleViewModel = new ChannelScheduleViewModel();
                    channelScheduleViewModel.ChannelScheduleCommand.Execute(channelId);
                    InvokePlayerScript(Constants.LiveTVPlayMode.PlayTV, channelPlayerID);
                    GrSchedule.DataContext = channelScheduleViewModel;
                    stkSchedule.DataContext = channelScheduleViewModel;
                    this.CurrentChannel = channelId;
                    ApplicationData.Settings.LiveTVChannel = channelId;

                    if (channelId.Equals(Constants.LinkNames.LiveTVNDTVHinduSchedule))
                    {
                        channelScheduleViewModel.CurrentShow = Constants.Constant.NotAvailableMsg;
                        channelScheduleViewModel.UpcomingShow = Constants.Constant.NotAvailableMsg;
                    }
                    isFirstTime = false;
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Invokes the script on player file.
        /// </summary>
        /// <param name="senderMethodName">The method call from which the script is being invoked.</param>
        /// <param name="methodName">The script name.</param>
        /// <param name="parameter">The parameter for the script.</param>
        private void InvokePlayerScript(string methodName, string parameter = null)
        {
            LiveTVBrowser.Visibility = Visibility.Hidden;
            if (string.IsNullOrEmpty(parameter))
            {
                LiveTVBrowser.InvokeScript(methodName);
            }
            else
            {
                LiveTVBrowser.InvokeScript(methodName, parameter);
            }
            LiveTVBrowser.Visibility = Visibility.Visible;
            this.AdControl.RefreshAdBanner();
        }
        
        /// <summary>
        /// Close button click event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Routed event args</param>      
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
            this.Close();
        }

        /// <summary>
        /// Open selected video.
        /// </summary>
        /// <param name="sender"> Sender object. </param>
        /// <param name="args"> Argument object. </param>
        private void VideoListSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs args)
        {
            StopVideo();
            if (ApplicationData.IsApplicationOnline)
            {                
                VideoItem video = RelatedVideoList.SelectedItem as VideoItem;
                if (null != video)
                {
                    VideoPlayer videoPlayer = new VideoPlayer(video);
                    videoPlayer.VideoPlayerClosed += (playerSender, playerArguments) =>
                    {                        
                        PlayLiveTv();
                    };                    
                    videoPlayer.Owner = this.Owner;                    
                    videoPlayer.ShowDialog();
                    
                    /* 
                     * To remove selection of this video from LIVE TV video gallery, 
                     * once user is back to live tv by closing video player.
                     */
                    RelatedVideoList.SelectedIndex = -1;
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Stop Video and Audio streaming.
        /// </summary>
        private void StopVideo()
        {
            InvokePlayerScript(Constants.LiveTVPlayMode.PlayVideoOnDemand, string.Empty);
        }

        /// <summary>
        /// Player Live TV.
        /// </summary>
        private void PlayLiveTv()
        {
            isFirstTime = true;
            LiveTVBrowser.Navigate(Utility.GetLink(Constants.LinkNames.LiveTVVideoPlayerLink));
            this.AdControl.RefreshAdBanner();
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
                this.SetWindowPosition();
                LandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                PortraitView();
            }
        }

        /// <summary>
        /// A Method which sets the window to Landscape orientation with all the required properties
        /// </summary>
        private void LandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            GrLiveTV.Height = double.NaN;
            ChannelLabel.SetValue(Grid.ColumnSpanProperty, 1);
            ChannelButtons.SetValue(Grid.ColumnSpanProperty, 1);
            ChannelButtons.SetValue(Grid.RowProperty, 0);
            PlayerColumn.Width = new GridLength(550, GridUnitType.Pixel);
            RelatedVideosColumn.Width = new GridLength(1, GridUnitType.Star);
            PlayerRow.Height = new GridLength(1, GridUnitType.Star);
            RelatedVideosRow.Height = GridLength.Auto;
            GrSchedule.SetValue(Grid.RowProperty, 0);
            GrSchedule.SetValue(Grid.ColumnProperty, 1);
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
            GrLiveTV.Height = 400;
            ChannelLabel.SetValue(Grid.ColumnSpanProperty, 2);
            ChannelButtons.SetValue(Grid.ColumnSpanProperty, 2);
            ChannelButtons.SetValue(Grid.RowProperty, 1);
            PlayerColumn.Width = new GridLength(1, GridUnitType.Star);
            RelatedVideosColumn.Width = new GridLength(0, GridUnitType.Pixel);
            PlayerRow.Height = GridLength.Auto;
            RelatedVideosRow.Height = new GridLength(1, GridUnitType.Star);
            GrSchedule.SetValue(Grid.RowProperty, 1);
            GrSchedule.SetValue(Grid.ColumnProperty, 0);
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

        #endregion Private methods
    }
}
