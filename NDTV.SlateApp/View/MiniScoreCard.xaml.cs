using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using SlateProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Delegate for Full ScoreCard click
    /// </summary>
    /// <param name="scoreCardViewModel">CricketFullScoreCardViewModel</param>
    public delegate void ShowFullScoreOfMiniScorecardEventHandler(CricketFullScorecardViewModel scorecardViewModel);

    /// <summary>
    /// Delegate for Full Commentary click
    /// </summary>
    /// <param name="scoreCardViewModel">CricketFullScoreCardViewModel</param>
    public delegate void ShowFullCommentaryOfMiniScorecardEventHandler(CricketFullScorecardViewModel scorecardViewModel);

    /// <summary>
    /// Interaction logic for MiniScoreCard.xaml
    /// </summary>
    public partial class MiniScorecard : UserControl
    {
        /// <summary>
        /// view model - cricket commentary
        /// </summary>
        private CricketCommentaryViewModel cricketCommentary;

        /// <summary>
        /// List of Live matches
        /// </summary>
        private List<CricketFixtures> liveMatches;

        /// <summary>
        /// match index in the Live match list
        /// </summary>
        private int matchIndex;

        /// <summary>
        /// event for Full ScoreCard click
        /// </summary>
        public event ShowFullScoreOfMiniScorecardEventHandler ShowFullScoreFromMiniScorecard;

        /// <summary>
        /// event for show full commentary
        /// </summary>
        public event ShowFullCommentaryOfMiniScorecardEventHandler ShowFullCommentaryFromMiniScorecard;

        /// <summary>
        /// Match file
        /// </summary>
        public CricketFixtures MatchFile
        {
            get { return (CricketFixtures)GetValue(MatchFileProperty); }
            set { SetValue(MatchFileProperty, value); }
        }

        /// <summary>
        /// CricketFullScoreCardViewModel
        /// </summary>
        private CricketFullScorecardViewModel fullScorecardViewModel;

        /// <summary>
        /// Current fixture
        /// </summary>
        private CricketFixtures fixture;


        /// <summary>
        /// CricketFullScorecardViewModelProperty
        /// </summary>
        public CricketFullScorecardViewModel CricketFullScorecardViewModelProperty
        {
            get { return (CricketFullScorecardViewModel)GetValue(CricketFullScorecardViewModelPropertyProperty); }
            set { SetValue(CricketFullScorecardViewModelPropertyProperty, value); }
        }

        /// <summary>
        /// Dependency property
        /// </summary>
        public static readonly DependencyProperty CricketFullScorecardViewModelPropertyProperty =
            DependencyProperty.Register("CricketFullScorecardViewModelProperty", typeof(CricketFullScorecardViewModel), typeof(MiniScorecard), new UIPropertyMetadata(OnViewModelChanged));

        /// <summary>
        /// Dependency property
        /// </summary>
        /// <param name="dependencyObject">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnViewModelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (null != dependencyObject)
            {
                (dependencyObject as MiniScorecard).UpdateViewModel();
            }
        }

        /// <summary>
        /// Dependency property
        /// </summary>
        public static readonly DependencyProperty MatchFileProperty =
            DependencyProperty.Register("MatchFile", typeof(CricketFixtures), typeof(MiniScorecard), new PropertyMetadata(OnMatchFileChanged));

        /// <summary>
        /// Dependency property
        /// </summary>
        /// <param name="dependencyObject">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMatchFileChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (null != dependencyObject)
            {
                (dependencyObject as MiniScorecard).UpdateMatchFile();
            }
        }

        /// <summary>
        /// when ever view model is getting set, update the view model of the current page(that is of MiniScore card)
        /// </summary>
        private void UpdateViewModel()
        {
            SetOrientation();
            if (null != CricketFullScorecardViewModelProperty && null != CricketFullScorecardViewModelProperty.MatchDetail && null != CricketFullScorecardViewModelProperty.MatchDetail.MatchFixtures)
            {
                if (CricketFullScorecardViewModelProperty.MatchDetail.MatchFixtures.IsLive)
                {
                    SetVisibility();
                    fullScorecardViewModel = CricketFullScorecardViewModelProperty;
                    this.fixture = CricketFullScorecardViewModelProperty.MatchDetail.MatchFixtures;
                    FullScorecardViewModelLoaded();
                }
            }
        }

        /// <summary>
        /// Calling the mini scorecard view model
        /// </summary>
        private void UpdateMatchFile()
        {
            SetOrientation();
            if (null == this.MatchFile || null == this.MatchFile.MatchFile)
            {
                this.MatchGrid.Visibility = Visibility.Collapsed;
                this.NoMatchInProgressGrid.Visibility = Visibility.Visible;
                this.preLoader.Visibility = Visibility.Collapsed;
            }

            if (null != this.MatchFile && this.MatchFile.IsLive)
            {
                if (null != fullScorecardViewModel)
                {
                    fullScorecardViewModel.Dispose();
                }
                SetVisibility();
                this.fixture = MatchFile;
                fullScorecardViewModel = new CricketFullScorecardViewModel(this.MatchFile, CricketFixture.Live, FullScorecardViewModelLoaded);
            }
        }

        /// <summary>
        /// set the visibility of controls
        /// </summary>
        private void SetVisibility()
        {
            this.NoMatchInProgressGrid.Visibility = Visibility.Collapsed;
            this.MatchGrid.Visibility = Visibility.Visible;
            this.preLoader.Visibility = Visibility.Visible;
            this.MatchGrid.Visibility = Visibility.Collapsed;
            this.LiveMatchTicker.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Call back method for Full Score card view model loaded
        /// </summary>
        private void FullScorecardViewModelLoaded()
        {
            if (null != cricketCommentary)
            {
                cricketCommentary.Dispose();
                cricketCommentary = null;
            }
            if (null != fullScorecardViewModel && null != fullScorecardViewModel.SelectedInnings && null != fullScorecardViewModel.SelectedInnings.InningsNumber)
            {
                SetOrientation();
                cricketCommentary = new CricketCommentaryViewModel(Helper.FormatInningsNumber(fullScorecardViewModel.SelectedInnings.InningsNumber), Entities.CricketCommentary.Short, fullScorecardViewModel.MatchDetail.MatchFixtures, CommentaryLoaded);
            }
            else
            {
                CommentaryLoaded();
            }
        }

        /// <summary>
        /// Call back method for Commentary loaded
        /// </summary>
        private void CommentaryLoaded()
        {
            if (null != ApplicationData.MatchFixtureResponse && null != ApplicationData.MatchFixtureResponse.LiveMatchList)
            {
                liveMatches = ApplicationData.MatchFixtureResponse.LiveMatchList.ToList();
            }
            this.DataContext = fullScorecardViewModel;
            this.CommentaryGrid.DataContext = cricketCommentary;
            SetControlsVisibility();
        }

        /// <summary>
        /// Visibility of the controls
        /// </summary>
        private void SetControlsVisibility()
        {
            this.preLoader.Visibility = Visibility.Collapsed;
            SetLiveMatchTicker();
            if (null != this.fixture && this.fixture.IsLive && null != fullScorecardViewModel && fullScorecardViewModel.LiveMatchList.Count > 1)
            {
                this.LiveMatchTicker.Visibility = Visibility.Visible;
            }
            if (null != cricketCommentary && null != cricketCommentary.CommentaryList && cricketCommentary.CommentaryList.Count > 0)
            {
                this.NoCommentaryContainer.Visibility = Visibility.Collapsed;
                this.CommentryContainer.Visibility = Visibility.Visible;
                this.MatchGrid.Visibility = Visibility.Visible;
            }
            else
            {
                this.NoCommentaryContainer.Visibility = Visibility.Visible;
                this.CommentryContainer.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MiniScorecard()
        {
            this.InitializeComponent();
            if (null != App.Current)
            {
                (App.Current as App).OnApplicationOnline += OnApplicationOnline;
                (App.Current as App).OnApplicationOffline += OnApplicationOffline;
            }
            MatchFile = new CricketFixtures();
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
        /// Show full score card button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ShowFullScoreFromMiniCard(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != ShowFullScoreFromMiniScorecard && null != fullScorecardViewModel)
                {
                    ShowFullScoreFromMiniScorecard(fullScorecardViewModel);
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Show Full Commentary click event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ShowFullCommentary(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != ShowFullCommentaryFromMiniScorecard && null != fullScorecardViewModel)
                {
                    ShowFullCommentaryFromMiniScorecard(fullScorecardViewModel);
                }
            }
            else
            {
                (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Set orientation to the user control.
        /// </summary>
        public void SetOrientation()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                DataContainerGridColumnOne.Width = new GridLength(400, GridUnitType.Pixel);
                RelatedVideosGrid.SetValue(Grid.ColumnProperty, 1);
                RelatedVideosGrid.SetValue(Grid.RowProperty, 1);
                RelatedVideosAndNews.SetOrientation();
            }
            else
            {
                DataContainerGridColumnOne.Width = new GridLength(0, GridUnitType.Pixel);
                RelatedVideosGrid.SetValue(Grid.ColumnProperty, 0);
                RelatedVideosGrid.SetValue(Grid.RowProperty, 2);
                RelatedVideosAndNews.SetOrientation();
            }
        }

        /// <summary>
        /// Select the currently displayed match
        /// </summary>
        private void SetLiveMatchTicker()
        {
            if (null != fullScorecardViewModel && null != this.MatchFile && this.fixture.IsLive)
            {
                if (fullScorecardViewModel.LiveMatchList.Count > 0 && fullScorecardViewModel.LiveMatchList.Any(matchFile => matchFile.MatchFile.Equals(this.fixture.MatchFile)))
                {
                    foreach (CricketFixtures cricketFixture in fullScorecardViewModel.LiveMatchList)
                    {
                        cricketFixture.IsSelected = cricketFixture.MatchFile.Equals(this.fixture.MatchFile) ? true : false;
                    }
                }
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
                string matchFile = liveMatchRadioButton.Tag.ToString();
                if (false == string.IsNullOrWhiteSpace(matchFile) && fullScorecardViewModel.LiveMatchList.Count > 0 && fullScorecardViewModel.LiveMatchList.Any(file => file.MatchFile.Equals(matchFile)) && 
                       false == MatchFile.Equals(fullScorecardViewModel.LiveMatchList.Where(file => file.MatchFile.Equals(matchFile)).FirstOrDefault()))
                {
                    this.AdvertisementControl.RefreshAdBanner();
                    MatchFile = fullScorecardViewModel.LiveMatchList.Where(file => file.MatchFile.Equals(matchFile)).FirstOrDefault();
                    this.fixture = MatchFile;
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