using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.SlateApp.ViewModel;
using NDTV.Utilities;
using SlateProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.View
{
    /// <summary>
    /// Delegate for full score card button click
    /// </summary>
    public delegate void ShowFullScorecardEventHandler(CricketFullScorecardViewModel fullScorecardViewModel);

    /// <summary>
    /// Interaction logic for CricketCommentary.xaml
    /// </summary>
    public partial class CricketCommentary : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CricketCommentary()
        {
            this.InitializeComponent();
            SetOrientation();
        }

        private CricketFullScorecardViewModel fullScorecardModelView;
        private CricketCommentaryViewModel commentaryViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullScorecardModelViewl">CricketFullScorecardViewModel</param>
        public CricketCommentary(CricketFullScorecardViewModel fullScorecardModelView)
        {
            this.fullScorecardModelView = fullScorecardModelView;
            LoadCommentary();
        }

        /// <summary>
        /// event for Close full commentary
        /// </summary>
        public event ShowFullScorecardEventHandler CloseFullCommentaryButtonClick;
        /// <summary>
        /// event for Show full score card
        /// </summary>
        public event ShowFullScorecardEventHandler ShowFullScorecardFromCommentaryButtonClick;

        /// <summary>
        /// Load commentary
        /// </summary>
        private void LoadCommentary()
        {
            this.InitializeComponent();
            SetOrientation();
            this.NoCommentaryContainer.Visibility = Visibility.Collapsed;
            this.preLoader.Visibility = Visibility.Visible;
            if (null != fullScorecardModelView && null != fullScorecardModelView.MatchDetail && null != fullScorecardModelView.MatchDetail.MatchFixtures)
            {
                if (null != commentaryViewModel)
                {
                    commentaryViewModel.Dispose();
                }
                commentaryViewModel = new CricketCommentaryViewModel(GetInningsName(), Entities.CricketCommentary.Detailed, fullScorecardModelView.MatchDetail.MatchFixtures, CommentaryLoadCompleted);
            }
        }

        /// <summary>
        /// Call back for commentary complete
        /// </summary>
        private void CommentaryLoadCompleted()
        {
            if (null != commentaryViewModel && null != commentaryViewModel.CommentaryList && commentaryViewModel.CommentaryList.Count > 0)
            {
                CommentaryListViewer.Visibility = Visibility.Visible;
                this.DataContext = commentaryViewModel;
            }
            else
            {
                this.NoCommentaryContainer.Visibility = Visibility.Visible;
                CommentaryListViewer.Visibility = Visibility.Collapsed;
                this.preLoader.Visibility = Visibility.Collapsed;
            }
            this.MatchHeader.DataContext = this.fullScorecardModelView.MatchDetail;
            this.MatchResult.DataContext = this.fullScorecardModelView;
        }

        /// <summary>
        /// Get the innings name
        /// </summary>
        /// <returns>innings name</returns>
        private string GetInningsName()
        {
            if (null != fullScorecardModelView && null != fullScorecardModelView.SelectedInnings)
            {
                return (Helper.FormatInningsNumber(fullScorecardModelView.SelectedInnings.InningsNumber));
            }
            return "1";
        }

        /// <summary>
        /// Close commentary button event handler
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void CloseCommentary(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != CloseFullCommentaryButtonClick)
                {
                    if (null != fullScorecardModelView && null != fullScorecardModelView.MatchDetail && null != fullScorecardModelView.MatchDetail.MatchFixtures && fullScorecardModelView.MatchDetail.MatchFixtures.IsLive)
                    {
                        CloseFullCommentaryButtonClick(fullScorecardModelView);
                    }
                    else
                    {
                        CloseFullCommentaryButtonClick(null);
                    }
                }
                if (null != commentaryViewModel)
                {
                    commentaryViewModel.Dispose();
                }
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// Show full score card button event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ShowFullScorecardFromFullCommentary(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                if (null != ShowFullScorecardFromCommentaryButtonClick)
                {
                    ShowFullScorecardFromCommentaryButtonClick(this.fullScorecardModelView);
                }
            }
            else if (null != App.Current)
            {
                (App.Current as App).DisplayErrorMessage(SlateProperties.Resources.ContentLoadFailureMessage, string.Empty, false, null);
            }
        }

        /// <summary>
        /// set orientation to the user control.
        /// </summary>
        public void SetOrientation()
        {
            if (ApplicationData.IsLandscapeOrientation)
            {
                RelatedVideosGrid.SetValue(Grid.RowSpanProperty, 2);
                RelatedVideosGrid.SetValue(Grid.ColumnProperty, 1);
                RelatedVideosGrid.SetValue(Grid.RowProperty, 0);
                TopGridColumnOne.Width = new GridLength(400, GridUnitType.Pixel);
                RelatedVideosAndNews.SetOrientation();
            }
            else
            {
                RelatedVideosGrid.SetValue(Grid.RowSpanProperty, 1);
                RelatedVideosGrid.SetValue(Grid.ColumnProperty, 0);
                RelatedVideosGrid.SetValue(Grid.RowProperty, 2);
                TopGridColumnOne.Width = new GridLength(0, GridUnitType.Pixel);
                RelatedVideosAndNews.SetOrientation();
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