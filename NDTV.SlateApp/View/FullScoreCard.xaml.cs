using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using SlateProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Delegate for full score card button click
    /// </summary>
    public delegate void OnFullScorecardEventHandler(CricketFullScorecardViewModel scorecard);

    /// <summary>
    /// Interaction logic for FullScoreCard.xaml
    /// </summary>
    public partial class FullScorecard : UserControl
    {
        /// <summary>
        /// CricketFullScoreCardViewModel
        /// </summary>
        private CricketFullScorecardViewModel scorecard;

        /// <summary>
        /// State of the match. Whether it is live or recent
        /// </summary>
        private CricketFixture matchState;

        /// <summary>
        /// CricketFixtures
        /// </summary>
        private CricketFixtures fixture;

        /// <summary>
        /// Event for close click
        /// </summary>
        public event OnFullScorecardEventHandler FullScorecardCloseClick;

        /// <summary>
        /// event for full score card button click
        /// </summary>
        public event OnFullScorecardEventHandler FullCommentaryFromFullScorecardClick;

        /// <summary>
        /// Constructor which takes filename of the match as parameter
        /// </summary>
        /// <param name="fileName">fileName of the match</param>
        /// <param name="state">whether match is recent or live</param>
        public FullScorecard(string fileName, CricketFixture state)
        {
            if (null != App.Current)
            {
                (App.Current as App).OnApplicationOnline += OnApplicationOnline;
                (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            }
            InitializeData(fileName, null, state);
        }

        /// <summary>
        /// Constructor which takes view model as parameter
        /// </summary>
        /// <param name="fullScorecardViewModel">CricketFullScorecardViewModel</param>
        public FullScorecard(CricketFullScorecardViewModel fullScorecardViewModel)
        {
            if (null != App.Current)
            {
                (App.Current as App).OnApplicationOnline += OnApplicationOnline;
                (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            }
            InitializeData(null, fullScorecardViewModel, CricketFixture.Live);
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
                if (null != LiveMatchTicker)
                {
                    LiveMatchTicker.IsEnabled = true;
                }
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
                if (null != LiveMatchTicker)
                {
                    LiveMatchTicker.IsEnabled = false;
                }
            }));
        }

        /// <summary>
        /// Initialize the page and set the data context
        /// </summary>
        /// <param name="fileName">fileName(link) of the current match </param>
        /// <param name="state">Cricket fixture </param>
        private void InitializeData(string fileName, CricketFullScorecardViewModel fullScorecardViewModel, CricketFixture state)
        {
            this.InitializeComponent();
            SetOrientation();
            SetDefaultVisibility();
            matchState = state;
            if (false == string.IsNullOrWhiteSpace(fileName))
            {
                if (null != scorecard)
                {
                    scorecard.Dispose();
                    scorecard = null;
                }
                this.preLoader.Visibility = Visibility.Visible;
                scorecard = new CricketFullScorecardViewModel(fileName, state, FullScorecardViewModelLoaded);
            }
            else if (null != fullScorecardViewModel)
            {
                scorecard = fullScorecardViewModel;
                scorecard.IsScorecardLoadingUnderProgress = false;
                FullScorecardViewModelLoaded();
            }
        }

        /// <summary>
        /// Call back for full score card loaded
        /// </summary>
        private void FullScorecardViewModelLoaded()
        {
            if (null != scorecard.MatchDetail && null != scorecard.MatchDetail.MatchFixtures)
            {
                this.fixture = scorecard.MatchDetail.MatchFixtures;
            }
            this.DataContext = scorecard;
            this.AdvertisementControl.RefreshAdBanner();
            SetLiveMatchTicker();
            ControlsVisibility();
        }

        /// <summary>
        /// Visibility of the controls
        /// </summary>
        private void ControlsVisibility()
        {
            this.preLoader.Visibility = Visibility.Collapsed;
            this.CloseButton.Visibility = Visibility.Visible;
            if (null != this.fixture && this.fixture.IsLive && null != scorecard && scorecard.LiveMatchList.Count > 1)
            {
                this.LiveMatchTicker.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Set the default visibility for controls
        /// </summary>
        private void SetDefaultVisibility()
        {
            this.CloseButton.Visibility = Visibility.Collapsed;
            this.LiveMatchTicker.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Select the currently displayed match
        /// </summary>
        private void SetLiveMatchTicker()
        {
            if (null != scorecard && null != this.fixture && this.fixture.IsLive)
            {
                if (scorecard.LiveMatchList.Count > 0 && scorecard.LiveMatchList.Any(matchFile => matchFile.MatchFile.Equals(this.fixture.MatchFile)))
                {
                    foreach (CricketFixtures cricketFixture in scorecard.LiveMatchList)
                    {
                        cricketFixture.IsSelected = cricketFixture.MatchFile.Equals(this.fixture.MatchFile) ? true : false;
                    }
                }
            }
        }

        /// <summary>
        /// Event of radio button selection
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void InningsSelector(object sender, RoutedEventArgs e)
        {
            RadioButton innings = (RadioButton)e.OriginalSource;
            if (null != innings.Tag)
            {
                string tag = innings.Tag.ToString();
                scorecard.InningsCommand.Execute(tag);
            }
        }

        /// <summary>
        /// Control's close button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != FullScorecardCloseClick)
                {
                    if (null != scorecard && null != scorecard.MatchDetail && null != scorecard.MatchDetail.MatchFixtures && scorecard.MatchDetail.MatchFixtures.IsLive)
                    {
                        FullScorecardCloseClick(scorecard);
                    }
                    else
                    {
                        FullScorecardCloseClick(null);
                    }
                }
                if (matchState.Equals(CricketFixture.Recent))
                {
                    scorecard.Dispose();
                    scorecard = null;
                }
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Full Commentary button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void FullCommentaryClick(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != FullCommentaryFromFullScorecardClick)
                {
                    FullCommentaryFromFullScorecardClick(this.scorecard);
                }
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(NDTV.SlateApp.Properties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Set orientation to the user control.
        /// </summary>
        public void SetOrientation()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                BowlerMainGrid.SetValue(Grid.ColumnProperty, 1);
                BowlerMainGrid.SetValue(Grid.RowProperty, 1);
                BowlerMainGrid.Margin = new Thickness(0, 25, 20, 0);
                ContainerGridColumnZero.Width = new GridLength(525, GridUnitType.Pixel);
                ContainerGridColumnOne.Width = new GridLength(1, GridUnitType.Star);
                ContainerGridRowThree.Height = new GridLength(0, GridUnitType.Auto);
            }
            else
            {
                BowlerMainGrid.SetValue(Grid.ColumnProperty, 0);
                BowlerMainGrid.SetValue(Grid.RowProperty, 3);
                BowlerMainGrid.Margin = new Thickness(20, 5, 20, 0);
                ContainerGridColumnZero.Width = new GridLength(1, GridUnitType.Star);
                ContainerGridColumnOne.Width = new GridLength(0, GridUnitType.Pixel);
                ContainerGridRowThree.Height = new GridLength(1, GridUnitType.Star);
            }
        }

        /// <summary>
        /// Selected event for Live match
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void LiveMatchSelected(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                RadioButton liveMatchRadioButton = (RadioButton)e.OriginalSource;
                if (null != liveMatchRadioButton.Tag)
                {
                    string matchFile = liveMatchRadioButton.Tag.ToString();
                    if (false == string.IsNullOrWhiteSpace(matchFile) && scorecard.LiveMatchList.Count > 0 && scorecard.LiveMatchList.Any(file => file.MatchFile.Equals(matchFile))
                        && false == this.fixture.Equals(scorecard.LiveMatchList.Where(file => file.MatchFile.Equals(matchFile)).FirstOrDefault()))
                    {
                        InitializeData(matchFile, null, CricketFixture.Live);
                    }
                }
                SetLiveMatchTicker();
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Visibility of the advertisement
        /// </summary>
        /// <param name="visible">bool - whether to hide ad or not</param>
        public void AdvertisementState(bool visible)
        {
            if (null != AdvertisementControl)
            {
                this.AdvertisementControl.ShowAdvertisement = visible;
            }
        }

    }
}