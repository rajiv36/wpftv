using System.Windows;
using System.Windows.Controls;
using NDTV.Controller;
using NDTV.Entities;
using NDTV.SlateApp.ViewModel;
using SlateProperties = NDTV.SlateApp.Properties;

namespace NDTV.SlateApp.View
{

    /// <summary>
    /// delegate which takes match file name
    /// </summary>
    /// <param name="scoreCardFileName"></param>
    public delegate void OnShowScorecardEventHandler(string scorecardFileName);

    /// <summary>
    /// Interaction logic for CricketCalendarControl.xaml
    /// </summary>
    public partial class CricketCalendarControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selectedTab">string</param>
        public CricketCalendarControl(string selectedTab)
        {
            this.InitializeComponent();
            LoadControl(selectedTab);
        }

        /// <summary>
        /// Reload the control
        /// </summary>
        /// <param name="selectedTab">string</param>
        public void ReloadControl(string selectedTab)
        {
            LoadControl(selectedTab);
            this.AdvertisementControl.RefreshAdBanner();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CricketCalendarControl()
        {
            this.InitializeComponent();
        }

        private CricketFixturesViewModel cricketFixtures;

        /// <summary>
        /// Load data based on selection of tab
        /// </summary>
        /// <param name="selectedTab">selected tab</param>
        private void LoadControl(string selectedTab)
        {
            SetOrientation();
            if (null != cricketFixtures)
            {
                cricketFixtures.Dispose();
            }
            cricketFixtures = new CricketFixturesViewModel();
            this.DataContext = cricketFixtures;
            switch (selectedTab)
            {
                case Constants.Constant.Recent:
                    {
                        RecentMatchGrid.Visibility = Visibility.Visible;
                        UpcomingMatchGrid.Visibility = Visibility.Collapsed;
                        break;
                    }
                case Constants.Constant.Upcoming:
                    {
                        UpcomingMatchGrid.Visibility = Visibility.Visible;
                        RecentMatchGrid.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        /// <summary>
        /// Event for Show score card click
        /// </summary>
        public event OnShowScorecardEventHandler ShowScorecardEvent;

        /// <summary>
        /// Scorecard button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ShowScorecard(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.IsApplicationOnline)
            {
                Button showScoreButton = (Button)e.OriginalSource;
                if (null != showScoreButton.Tag)
                {
                    if (null != ShowScorecardEvent)
                    {
                        ShowScorecardEvent(showScoreButton.Tag.ToString());
                    }
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
                LayoutGridColumnOne.Width = new GridLength(400, GridUnitType.Pixel);
                RightGrid.SetValue(Grid.RowProperty, 1);
                RightGrid.SetValue(Grid.ColumnProperty, 1);
                RelatedVideosAndNews.SetOrientation();
            }
            else
            {
                LayoutGridColumnOne.Width = new GridLength(0, GridUnitType.Pixel);
                RightGrid.SetValue(Grid.RowProperty, 2);
                RightGrid.SetValue(Grid.ColumnProperty, 0);
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
