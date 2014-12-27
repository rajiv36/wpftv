using System;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Interaction logic for CricketWindow.xaml
    /// </summary>
    public partial class CricketWindow : SlateWindow
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public CricketWindow()
        {
            LoadData(null);            
        }

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
                if (null != cricketMain)
                {
                    if (null != cricketMain.FullCricketCommentary)
                    {
                        cricketMain.FullCricketCommentary.AdvertisementState(false);
                    }
                    if (null != cricketMain.CricketCalendarControl)
                    {
                        cricketMain.CricketCalendarControl.AdvertisementState(false);
                    }
                    if (null != cricketMain.MiniScorecard)
                    {
                        cricketMain.MiniScorecard.AdvertisementState(false);
                    }
                    if (null != cricketMain.FullScore)
                    {
                        cricketMain.FullScore.AdvertisementState(false);
                    }
                }
            }
            else
            {
                ModalPopup.Visibility = Visibility.Collapsed;
                if (null != cricketMain.FullCricketCommentary)
                {
                    cricketMain.FullCricketCommentary.AdvertisementState(true);
                }
                if (null != cricketMain.CricketCalendarControl)
                {
                    cricketMain.CricketCalendarControl.AdvertisementState(true);
                }
                if (null != cricketMain.MiniScorecard)
                {
                    cricketMain.MiniScorecard.AdvertisementState(true);
                }
                if (null != cricketMain.FullScore)
                {
                    cricketMain.FullScore.AdvertisementState(true);
                }
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
                SwitchToLandscapeView();
            }
            else
            {
                this.SetWindowPosition();
                SwitchToPortraitView();
            }
        }

        /// <summary>
        /// Helper function for SwitchToLandscapeView.
        /// </summary>
        protected override void SwitchToLandscapeView()
        {
            this.Width = 950;
            this.Height = 550;
            if (null != cricketMain && null != cricketMain.FullCricketCommentary)
            {
                cricketMain.FullCricketCommentary.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.FullScore)
            {
                cricketMain.FullScore.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.CricketCalendarControl)
            {
                cricketMain.CricketCalendarControl.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.MiniScorecard)
            {
                cricketMain.MiniScorecard.SetOrientation();
            }

            SetWindowPosition();
        }

        /// <summary>
        /// Helper function for SwitchToPortraitView.
        /// </summary>
        protected override void SwitchToPortraitView()
        {
            this.Width = 550;
            this.Height = 950;
            if (null != cricketMain && null != cricketMain.FullCricketCommentary)
            {
                cricketMain.FullCricketCommentary.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.FullScore)
            {
                cricketMain.FullScore.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.CricketCalendarControl)
            {
                cricketMain.CricketCalendarControl.SetOrientation();
            }
            if (null != cricketMain && null != cricketMain.MiniScorecard)
            {
                cricketMain.MiniScorecard.SetOrientation();
            }
            SetWindowPosition();
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

        /// <summary>
        /// Constructor which takes match file name as parameter
        /// </summary>
        /// <param name="matchFileName">file name of the match</param>
        public CricketWindow(string matchFileName)
        {
            this.matchFileName = matchFileName;
            LoadData(matchFileName);
            ApplicationData.IsPopUpOpenValueChanged += IsPopUpOpenValueChanged;
            if (null != App.Current)
            {
                (App.Current as App).OnApplicationOnline += OnApplicationOnline;
                (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            }
        }

        /// <summary>
        /// Event fires when application comes online.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnApplicationOnline(object sender, EventArgs e)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(delegate()
            {
                TabsPanel.IsEnabled = true;
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
                TabsPanel.IsEnabled = false;
            }));
        }

        private CricketFixtures currentFixture;
        private CricketMainWindowViewModel cricketMain;

        /// <summary>
        /// current selected tab
        /// </summary>
        private string currentTab;

        /// <summary>
        /// Check if it is the first time page is loading
        /// </summary>
        private bool isFirstTime;

        /// <summary>
        /// Initializes data
        /// </summary>
        private void LoadData(string matchFileName)
        {
            isFirstTime = true;
            this.InitializeComponent();
            this.matchFileName = matchFileName;
            cricketMain = new CricketMainWindowViewModel(CricketWindowViewModelLoaded);
        }

        private string matchFileName;

        /// <summary>
        /// Call back for cricket calendar loaded
        /// </summary>
        private void CricketWindowViewModelLoaded()
        {
            if (isFirstTime)
            {
                currentFixture = cricketMain.LoadLiveMatchFile(matchFileName);
                cricketMain.LoadMiniScorecard();
                cricketMain.CricketCalendarControl.ShowScorecardEvent += ShowFullScorecard;
                cricketMain.MiniScorecard.ShowFullScoreFromMiniScorecard += ShowFullScoreFromMiniScorecard;
                cricketMain.MiniScorecard.ShowFullCommentaryFromMiniScorecard += ShowFullCommentaryFromMiniScorecard;
                isFirstTime = false;
                if (false == string.IsNullOrWhiteSpace(this.matchFileName))
                {
                    this.LiveTab.IsChecked = true;
                    currentTab = Constants.Constant.Live;
                    this.scorecardContainer.Child = cricketMain.MiniScorecard;
                }
                else
                {
                    this.RecentTab.IsChecked = true;
                    ApplicationData.SetCurrentItem(string.Empty, string.Empty, string.Empty, string.Empty,
                    NDTV.SlateApp.Properties.Resources.CricketFixturesLink, ShareMediaType.RecentMatches); 
                    currentTab = Constants.Constant.Recent;
                    this.scorecardContainer.Child = cricketMain.CricketCalendarControl;
                }
            }
        }

        /// <summary>
        /// Full Commentary click from miniscore card
        /// </summary>
        /// <param name="scorecardViewModel">CricketFullScorecardViewModel</param>
        private void ShowFullCommentaryFromMiniScorecard(CricketFullScorecardViewModel scorecardViewModel)
        {
            DisplayFullCommentary(scorecardViewModel);
        }

        /// <summary>
        /// Display full score card
        /// </summary>
        /// <param name="scoreCardViewModel">CricketFullScoreCardViewModel</param>
        private void DisplayFullCommentary(CricketFullScorecardViewModel scoreCardViewModel)
        {
            this.TabsPanel.Visibility = Visibility.Collapsed;
            cricketMain.ShowFullCommentary(scoreCardViewModel);
            cricketMain.FullCricketCommentary.CloseFullCommentaryButtonClick += CloseFullCommentaryButtonClick;
            cricketMain.FullCricketCommentary.ShowFullScorecardFromCommentaryButtonClick += ShowFullScorecardFromCommentaryButtonClick;
            this.scorecardContainer.Child = cricketMain.FullCricketCommentary;
        }

        /// <summary>
        /// Show full score card from full commentary page
        /// Button click handler
        /// </summary>
        /// <param name="fullScoreCardViewModel">CricketFullScorecardViewModel</param>
        private void ShowFullScorecardFromCommentaryButtonClick(CricketFullScorecardViewModel fullScorecardViewModel)
        {
            DisplayFullScorecard(null, fullScorecardViewModel);
        }

        /// <summary>
        /// event of the close button of Full score commentary.
        /// </summary>
        /// <param name="fullScorecardViewModel">CricketFullScorecardViewModel</param>
        private void CloseFullCommentaryButtonClick(CricketFullScorecardViewModel fullScorecardViewModel)
        {
            this.TabsPanel.Visibility = Visibility.Visible;
            if (null != fullScorecardViewModel && fullScorecardViewModel.MatchDetail.MatchFixtures.IsLive)
            {
                if (currentTab.Equals(Constants.Constant.Live))
                {
                    cricketMain.MiniScorecard.CricketFullScorecardViewModelProperty = fullScorecardViewModel;
                    this.scorecardContainer.Child = cricketMain.MiniScorecard;
                }
                else
                {
                    ReloadPreviousState();
                }
            }
            else
            {
                ReloadPreviousState();
            }
        }

        /// <summary>
        /// show full score card from mini score card
        /// </summary>
        /// <param name="scorecardViewModel">CricketFullScoreCardViewModel</param>
        private void ShowFullScoreFromMiniScorecard(CricketFullScorecardViewModel scorecardViewModel)
        {
            DisplayFullScorecard(null, scorecardViewModel);
        }

        /// <summary>
        /// Close event of full score card control
        /// </summary>
        /// <param name="fullScorecardViewModel">CricketFullScorecardViewModel</param>
        private void FullScorecardCloseClick(CricketFullScorecardViewModel fullScorecardViewModel)
        {
            this.TabsPanel.Visibility = Visibility.Visible;
            if (null != fullScorecardViewModel && fullScorecardViewModel.MatchDetail.MatchFixtures.IsLive)
            {
                if (currentTab.Equals(Constants.Constant.Live))
                {
                    cricketMain.MiniScorecard.CricketFullScorecardViewModelProperty = fullScorecardViewModel;
                    this.scorecardContainer.Child = cricketMain.MiniScorecard;
                }
                else
                {
                    ReloadPreviousState();
                }
            }
            else
            {
                ReloadPreviousState();
            }
        }

        /// <summary>
        /// On close of Full Score card reload the previous usercontrol
        /// </summary>
        private void ReloadPreviousState()
        {
            if (currentTab.Equals(Constants.Constant.Upcoming))
            {
                this.UpcomingTab.IsChecked = true;
                LoadFixtureTabs(Constants.Constant.Upcoming);
            }
            else if (currentTab.Equals(Constants.Constant.Live))
            {
                cricketMain.MiniScorecard.MatchFile = currentFixture;
                this.scorecardContainer.Child = cricketMain.MiniScorecard;
            }
            else
            {
                this.RecentTab.IsChecked = true;
                LoadFixtureTabs(Constants.Constant.Recent);
            }
        }

        /// <summary>
        /// Show full score card for recent matches
        /// </summary>
        /// <param name="scoreCardFile">match file name</param>
        private void ShowFullScorecard(string scoreCardFile)
        {
            DisplayFullScorecard(scoreCardFile, null);
        }

        /// <summary>
        /// Display full score card
        /// </summary>
        /// <param name="fileName">fileName of the match</param>
        /// <param name="fullScorecardViewModel">CricketFullScoreCardViewModel</param>
        private void DisplayFullScorecard(string fileName, CricketFullScorecardViewModel fullScorecardViewModel)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                this.TabsPanel.Visibility = Visibility.Collapsed;
                if (null == fileName)
                {
                    cricketMain.ShowFullScorecard(null, fullScorecardViewModel);
                }
                else
                {
                    cricketMain.ShowFullScorecard(fileName, null);
                }
                cricketMain.FullScore.FullScorecardCloseClick += FullScorecardCloseClick;
                cricketMain.FullScore.FullCommentaryFromFullScorecardClick += FullCommentaryFromFullScorecardClick;
                this.scorecardContainer.Child = cricketMain.FullScore;
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }

        }

        /// <summary>
        /// Show full commentary from full score card
        /// </summary>
        /// <param name="scorecardViewModel"></param>
        private void FullCommentaryFromFullScorecardClick(CricketFullScorecardViewModel scorecardViewModel)
        {
            DisplayFullCommentary(scorecardViewModel);
        }

        /// <summary>
        /// Event for selected radio button
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void TabSelected(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Content.ToString();
            currentTab = name.ToString();
            if (name.Equals(Constants.Constant.Recent) || name.Equals(Constants.Constant.Upcoming))
            {
                if (this.headerControl.FooterVisible == Visibility.Collapsed)
                {
                    this.headerControl.FooterVisible = Visibility.Visible;
                }
                ShareMediaType shareType = name.Equals(Constants.Constant.Recent) ? ShareMediaType.RecentMatches : ShareMediaType.UpcomingMatches;
                ApplicationData.SetCurrentItem(string.Empty, string.Empty, string.Empty, string.Empty,
                    NDTV.SlateApp.Properties.Resources.CricketFixturesLink, shareType); 

                LoadFixtureTabs(name);
            }
            else if (name.Equals(Constants.Constant.Live))
            {
                if (null != currentFixture)
                {
                    cricketMain.MiniScorecard.MatchFile = currentFixture;
                }
                else
                {
                    this.headerControl.FooterVisible = Visibility.Collapsed;
                }
                this.scorecardContainer.Child = cricketMain.MiniScorecard;
            }
        }

        /// <summary>
        /// Load Recent or Upcoming tab
        /// </summary>
        /// <param name="schedule">whether recent or Upcoming tab to be selected</param>
        private void LoadFixtureTabs(string schedule)
        {
            if (null != cricketMain.CricketCalendarControl)
            {
                cricketMain.CricketCalendarControl.ReloadControl(schedule);
                this.scorecardContainer.Child = cricketMain.CricketCalendarControl;
            }
        }

        /// <summary>
        /// Cricket window close button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void CricketWindowCloseButton(object sender, RoutedEventArgs e)
        {
            ApplicationData.IsPopUpOpenValueChanged -= IsPopUpOpenValueChanged;
            this.Dispose();
            this.Close();
        }

        /// <summary>
        /// Disposes all the objects
        /// </summary>
        protected override void DisposeResources()
        {
            cricketMain.Dispose();
        }

    }
}