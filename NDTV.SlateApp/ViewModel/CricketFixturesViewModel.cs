using System.Collections.ObjectModel;
using NDTV.Controller;
using NDTV.Entities;

namespace NDTV.SlateApp.ViewModel
{
    /// <summary>
    /// View model for recent, upcoming and live matches
    /// </summary>
    public class CricketFixturesViewModel : ViewModelBase
    {
        /// <summary>
        /// Cricket fixture response
        /// </summary>
        private CricketFixturesResponse fixtureResponse;

        /// <summary>
        /// Cricket fixture response
        /// </summary>
        public CricketFixturesResponse FixtureResponse
        {
            get { return fixtureResponse; }
            set 
            {
                fixtureResponse = value;
                OnPropertyChanged("FixtureResponse");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CricketFixturesViewModel()
        {
            FixtureResponse = ApplicationData.MatchFixtureResponse;
        }

        /// <summary>
        /// List of live cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> LiveMatchList
        {
            get { return FixtureResponse.LiveMatchList; }
        }

        /// <summary>
        /// List of recent cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> RecentMatchList
        {
            get { return FixtureResponse.RecentMatchList; }
        }

        /// <summary>
        /// List of upcoming cricket matches
        /// </summary>
        public ObservableCollection<CricketFixtures> UpcomingMatchList
        {
            get { return FixtureResponse.UpcomingMatchList; }
        }

        /// <summary>
        /// Disposes all the objects in the view model
        /// </summary>
        protected override void DisposeResources()
        {
            this.FixtureResponse = null;
        }
    }
}
